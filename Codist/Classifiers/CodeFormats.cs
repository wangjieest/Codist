﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Windows;

namespace Codist.Classifiers
{
	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpLocalFieldName)]
	[Name(Constants.CSharpLocalFieldName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class LocalFieldFormat : ClassificationFormatDefinition
	{
		public LocalFieldFormat() {
			DisplayName = "Codist: local field";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpConstFieldName)]
	[Name(Constants.CSharpConstFieldName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ConstFieldFormat : ClassificationFormatDefinition
	{
		public ConstFieldFormat() {
			DisplayName = "Codist: const field";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpReadOnlyFieldName)]
	[Name(Constants.CSharpReadOnlyFieldName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ReadOnlyFieldFormat : ClassificationFormatDefinition
	{
		public ReadOnlyFieldFormat() {
			DisplayName = "Codist: readonly field";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpParameterName)]
	[Name(Constants.CSharpParameterName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ParameterFormat : ClassificationFormatDefinition
	{
		public ParameterFormat() {
			DisplayName = "Codist: parameter";
			IsItalic = true;
			ForegroundColor = System.Windows.Media.Colors.MediumPurple;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpTypeParameterName)]
	[Name(Constants.CSharpTypeParameterName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class TypeParameterFormat : ClassificationFormatDefinition
	{
		public TypeParameterFormat() {
			DisplayName = "Codist: type parameter";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpNamespaceName)]
	[Name(Constants.CSharpNamespaceName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class NamespaceFormat : ClassificationFormatDefinition
	{
		public NamespaceFormat() {
			DisplayName = "Codist: namespace";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpExtensionMethodName)]
	[Name(Constants.CSharpExtensionMethodName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ExtensionMethodFormat : ClassificationFormatDefinition
	{
		public ExtensionMethodFormat() {
			DisplayName = "Codist: extension method";
			IsItalic = true;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpExternMethodName)]
	[Name(Constants.CSharpExternMethodName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ExternMethodFormat : ClassificationFormatDefinition
	{
		public ExternMethodFormat() {
			DisplayName = "Codist: extern method";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpMethodName)]
	[Name(Constants.CSharpMethodName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class MethodFormat : ClassificationFormatDefinition
	{
		public MethodFormat() {
			DisplayName = "Codist: method";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpEventName)]
	[Name(Constants.CSharpEventName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class EventFormat : ClassificationFormatDefinition
	{
		public EventFormat() {
			DisplayName = "Codist: event";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpPropertyName)]
	[Name(Constants.CSharpPropertyName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class PropertyFormat : ClassificationFormatDefinition
	{
		public PropertyFormat() {
			DisplayName = "Codist: property";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpFieldName)]
	[Name(Constants.CSharpFieldName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class FieldFormat : ClassificationFormatDefinition
	{
		public FieldFormat() {
			DisplayName = "Codist: field";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpAliasNamespaceName)]
	[Name(Constants.CSharpAliasNamespaceName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class AliasNamespaceFormat : ClassificationFormatDefinition
	{
		public AliasNamespaceFormat() {
			DisplayName = "Codist: alias namespace";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpConstructorMethodName)]
	[Name(Constants.CSharpConstructorMethodName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class ConstructorMethodFormat : ClassificationFormatDefinition
	{
		public ConstructorMethodFormat() {
			DisplayName = "Codist: constructor method";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpDeclarationName)]
	[Name(Constants.CSharpDeclarationName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class DeclarationFormat : ClassificationFormatDefinition
	{
		public DeclarationFormat() {
			DisplayName = "Codist: declaration";
			FontRenderingSize = 20;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpNestedDeclarationName)]
	[Name(Constants.CSharpNestedDeclarationName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class NestedDeclarationFormat : ClassificationFormatDefinition
	{
		public NestedDeclarationFormat() {
			DisplayName = "Codist: nested declaration";
			FontRenderingSize = 16;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpStaticMemberName)]
	[Name(Constants.CSharpStaticMemberName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class StaticMemberFormat : ClassificationFormatDefinition
	{
		public StaticMemberFormat() {
			DisplayName = "Codist: static member";
			TextDecorations = new TextDecorationCollection(System.Windows.TextDecorations.Underline);
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpOverrideMemberName)]
	[Name(Constants.CSharpOverrideMemberName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class OverrideMemberFormat : ClassificationFormatDefinition
	{
		public OverrideMemberFormat() {
			DisplayName = "Codist: override member";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpAbstractMemberName)]
	[Name(Constants.CSharpAbstractMemberName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class AbstractMemberFormat : ClassificationFormatDefinition
	{
		public AbstractMemberFormat() {
			DisplayName = "Codist: abstract member";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpVirtualMemberName)]
	[Name(Constants.CSharpVirtualMemberName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class VirtualMemberFormat : ClassificationFormatDefinition
	{
		public VirtualMemberFormat() {
			DisplayName = "Codist: virtual member";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpSealedClassName)]
	[Name(Constants.CSharpSealedClassName)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class SealedClassFormat : ClassificationFormatDefinition
	{
		public SealedClassFormat() {
			DisplayName = "Codist: sealed class";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpLabel)]
	[Name(Constants.CSharpLabel)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class LabelFormat : ClassificationFormatDefinition
	{
		public LabelFormat() {
			DisplayName = "Codist: label";
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = Constants.CSharpAttributeNotation)]
	[Name(Constants.CSharpAttributeNotation)]
	[UserVisible(false)]
	[Order(After = Priority.High)]
	internal sealed class AttributeNotationFormat : ClassificationFormatDefinition
	{
		public AttributeNotationFormat() {
			DisplayName = "Codist: attribute notation";
		}
	}
}