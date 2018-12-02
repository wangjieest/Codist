﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Codist
{
	sealed class SemanticContext
	{
		readonly IWpfTextView _TextView;
		VersionStamp _Version;
		SyntaxNode _Node, _NodeIncludeTrivia;

		public SemanticContext(IWpfTextView textView) {
			_TextView = textView;
		}

		public Document Document { get; private set; }
		public SemanticModel SemanticModel { get; private set; }
		public CompilationUnitSyntax Compilation { get; private set; }
		public SyntaxNode Node => _Node != null && _Node.Span.Contains(Position)
			? _Node
			: (_Node = GetNode(Position, false, false));
		public SyntaxNode NodeIncludeTrivia {
			get {
				return _NodeIncludeTrivia != null && _NodeIncludeTrivia.Span.Contains(Position)
					? _NodeIncludeTrivia
					: (_NodeIncludeTrivia = GetNode(Position, true, true));
			}
		}
		public SyntaxToken Token { get; private set; }
		public ISymbol Symbol { get; private set; }
		public int Position { get; set; }

		public SyntaxNode GetNode(int position, bool includeTrivia, bool deep) {
			SyntaxNode node = Compilation.FindNode(Token.Span, includeTrivia, deep);
			SeparatedSyntaxList<VariableDeclaratorSyntax> variables;
			if (node.IsKind(SyntaxKind.FieldDeclaration) || node.IsKind(SyntaxKind.EventFieldDeclaration)) {
				variables = (node as BaseFieldDeclarationSyntax).Declaration.Variables;
			}
			else if (node.IsKind(SyntaxKind.VariableDeclaration)) {
				variables = (node as VariableDeclarationSyntax).Variables;
			}
			else if (node.IsKind(SyntaxKind.LocalDeclarationStatement)) {
				variables = (node as LocalDeclarationStatementSyntax).Declaration.Variables;
			}
			else {
				return node;
			}
			foreach (var variable in variables) {
				if (variable.Span.Contains(position)) {
					return node;
				}
			}
			return node.FullSpan.Contains(position) ? node : null;
		}
		public SyntaxTrivia GetNodeTrivia() {
			if (Node != null) {
				var triviaList = Token.HasLeadingTrivia ? Token.LeadingTrivia
								: Token.HasTrailingTrivia ? Token.TrailingTrivia
								: default;
				if (triviaList.Equals(SyntaxTriviaList.Empty) == false
					&& triviaList.FullSpan.Contains(Position)) {
					return triviaList.FirstOrDefault(i => i.Span.Contains(Position));
				}
			}
			return default;
		}

		public Task<ISymbol> GetSymbolAsync(int position, CancellationToken cancellationToken = default) {
			return Microsoft.CodeAnalysis.FindSymbols.SymbolFinder.FindSymbolAtPositionAsync(Document, position, cancellationToken);
		}

		/// <summary>
		/// Locate symbol in case when the semantic model has been changed.
		/// </summary>
		public async Task<ISymbol> RelocateSymbolAsync(ISymbol symbol, CancellationToken cancellationToken = default) {
			if (symbol.ContainingAssembly.GetSourceType() == AssemblySource.Metadata) {
				return symbol;
			}
			await UpdateAsync(cancellationToken);
			var path = symbol.DeclaringSyntaxReferences.FirstOrDefault(r => r.SyntaxTree != null);
			var doc = Document.Project.GetDocument(path.SyntaxTree.FilePath);
			return Microsoft.CodeAnalysis.FindSymbols.SymbolFinder.FindSimilarSymbols(symbol, (await doc.GetSemanticModelAsync(cancellationToken)).Compilation, cancellationToken)
				.FirstOrDefault() ?? symbol;
		}

		/// <summary>
		/// Roughly finds a new <see cref="SyntaxNode"/> in updated semantic model corresponding to the member or type declaration node.
		/// </summary>
		/// <param name="node">The old node.</param>
		/// <returns>The new node.</returns>
		public SyntaxNode RelocateDeclarationNode(SyntaxNode node) {
			if (node.SyntaxTree == SemanticModel.SyntaxTree) {
				// the syntax tree is the same (not changed)
				return node;
			}
			if (node.IsKind(SyntaxKind.VariableDeclarator)) {
				node = node.Parent.Parent;
			}
			if (node is MemberDeclarationSyntax == false) {
				return null;
			}
			var nodeFilePath = node.SyntaxTree.FilePath;
			var root = Compilation;
			if (String.Equals(nodeFilePath, SemanticModel.SyntaxTree.FilePath, StringComparison.OrdinalIgnoreCase) == false) {
				// not the same document
				if ((root = FindDocument(nodeFilePath)?.GetSemanticModelAsync().Result.SyntaxTree.GetCompilationUnitRoot()) == null) {
					// document no longer exists
					return null;
				}
			}
			var matches = new List<MemberDeclarationSyntax>(3);
			var s = node.GetDeclarationSignature(node.SpanStart);
			foreach (var item in root.Members) {
				MatchDeclarationNode(item, matches, s, node);
			}
			if (matches.Count == 1) {
				return matches[0];
			}
			if (matches.Count == 0) {
				return null;
			}
			var match = matches[0];
			matches = matches.FindAll(i => i.MatchSignature(node));
			if (matches.Count == 1) {
				return matches[0];
			}
			if (matches.Count == 0) {
				return match;
			}
			matches = matches.FindAll(i => i.MatchAncestorDeclaration(node));
			if (matches.Count >= 1) {
				return matches[0];
			}
			return match;
		}

		Document FindDocument(string docPath) {
			foreach (var item in Document.Project.Documents) {
				if (String.Equals(item.FilePath, docPath, StringComparison.OrdinalIgnoreCase)) {
					return item;
				}
			}
			return null;
		}

		static void MatchDeclarationNode(MemberDeclarationSyntax member, List<MemberDeclarationSyntax> matches, string signature, SyntaxNode node) {
			if (member.Kind() == node.Kind()
				&& member.GetDeclarationSignature() == signature) {
				matches.Add(member);
			}
			if (member.IsKind(SyntaxKind.NamespaceDeclaration)) {
				var ns = member as NamespaceDeclarationSyntax;
				foreach (var item in ns.Members) {
					MatchDeclarationNode(item, matches, signature, node);
				}
			}
			else if (member is TypeDeclarationSyntax) {
				var t = member as TypeDeclarationSyntax;
				foreach (var item in t.Members) {
					MatchDeclarationNode(item, matches, signature, node);
				}
			}
			else if (node.IsKind(SyntaxKind.EnumMemberDeclaration) && member.IsKind(SyntaxKind.EnumDeclaration)) {
				var e = member as EnumDeclarationSyntax;
				foreach (var item in e.Members) {
					MatchDeclarationNode(item, matches, signature, node);
				}
			}
		}

		public async Task<ISymbol> GetSymbolAsync(SyntaxNode node, CancellationToken cancellationToken = default) {
			var sm = SemanticModel;
			if (node.SyntaxTree != sm.SyntaxTree) {
				var doc = Document.Project.Solution.GetDocument(node.SyntaxTree);
				if (doc == null) {
					var nodeFilePath = node.SyntaxTree.FilePath;
					doc = Document.FilePath == nodeFilePath ? Document : Document.Project.Documents.FirstOrDefault(d => String.Equals(d.FilePath, nodeFilePath, StringComparison.OrdinalIgnoreCase));
					if (doc == null) {
						return null;
					}
					sm = await doc.GetSemanticModelAsync(cancellationToken);
					if (node.SpanStart >= sm.SyntaxTree.Length) {
						return null;
					}
					var newNode = sm.SyntaxTree.GetCompilationUnitRoot(cancellationToken).FindNode(new TextSpan(node.SpanStart, 0));
					//todo find out the new node
					if (newNode.IsKind(node.Kind())) {
						node = newNode;
					}
					else {
						return null;
					}
				}
				sm = await doc.GetSemanticModelAsync(cancellationToken);
			}
			var info = sm.GetSymbolInfo(node, cancellationToken);
			if (info.Symbol != null) {
				return info.Symbol;
			}
			var symbol = sm.GetDeclaredSymbol(node, cancellationToken);
			if (symbol != null) {
				return symbol;
			}
			var type = sm.GetTypeInfo(node, cancellationToken);
			if (type.Type != null) {
				return type.Type;
			}
			return null;
		}

		public async Task<ISymbol> GetSymbolAsync(CancellationToken cancellationToken) {
			return Node == null
				? null
				: await GetSymbolAsync(Position, cancellationToken);
		}

		public async Task<bool> UpdateAsync(CancellationToken cancellationToken) {
			try {
				var doc = _TextView.TextSnapshot.GetOpenDocumentInCurrentContextWithChanges();
				if (doc != Document) {
					Document = doc;
					SemanticModel = await Document.GetSemanticModelAsync(cancellationToken);
					Compilation = SemanticModel.SyntaxTree.GetCompilationUnitRoot(cancellationToken);
				}
				return true;
			}
			catch (NullReferenceException) {
				ResetNodeInfo();
			}
			return false;
		}

		public async Task<bool> UpdateAsync(int position, CancellationToken cancellationToken) {
			bool versionChanged = false;
			try {
				Document = _TextView.TextSnapshot.GetOpenDocumentInCurrentContextWithChanges();
				var ver = await Document.GetTextVersionAsync(cancellationToken);
				if (versionChanged = ver != _Version) {
					_Version = ver;
					SemanticModel = await Document.GetSemanticModelAsync(cancellationToken);
					Compilation = SemanticModel.SyntaxTree.GetCompilationUnitRoot(cancellationToken);
					ResetNodeInfo();
				}
			}
			catch (NullReferenceException) {
				ResetNodeInfo();
				return false;
			}
			Position = position;
			try {
				if (versionChanged || Token.Span.Contains(position) == false) {
					Token = Compilation.FindToken(position, true);
					_Node = _NodeIncludeTrivia = null;
				}
			}
			catch (ArgumentOutOfRangeException) {
				ResetNodeInfo();
				return false;
			}
			return true;
		}

		private void ResetNodeInfo() {
			_Node = _NodeIncludeTrivia = null;
			Token = default;
		}
	}
}
