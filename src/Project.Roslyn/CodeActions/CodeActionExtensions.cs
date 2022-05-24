using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Tags;
using Glyph = RoslynPad.Roslyn.Completion.Glyph;

namespace RoslynPad.Roslyn.CodeActions
{
    public static class CodeActionExtensions
    {
        public static bool HasCodeActions(this CodeAction codeAction)
        {
            if (codeAction == null) throw new ArgumentNullException(nameof(codeAction));

            return !codeAction.NestedCodeActions.IsDefaultOrEmpty;
        }

        public static ImmutableArray<CodeAction> GetCodeActions(this CodeAction codeAction)
        {
            if (codeAction == null) throw new ArgumentNullException(nameof(codeAction));

            return codeAction.NestedCodeActions;
        }

        public static Glyph GetGlyph(this CodeAction codeAction)
        {
            if (codeAction == null) throw new ArgumentNullException(nameof(codeAction));

            return GetGlyph(codeAction.Tags);
        }

        public static Glyph GetGlyph(ImmutableArray<string> tags)
        {
            foreach (var tag in tags)
            {
                switch (tag)
                {
                    case WellKnownTags.Assembly:
                        return Glyph.Assembly;

                    case WellKnownTags.File:
                        return tags.Contains(LanguageNames.VisualBasic) ? Glyph.BasicFile : Glyph.CSharpFile;

                    case WellKnownTags.Project:
                        return tags.Contains(LanguageNames.VisualBasic) ? Glyph.BasicProject : Glyph.CSharpProject;

                    case WellKnownTags.Class:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.ClassProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.ClassPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.ClassInternal;
                            default:
                                return Glyph.ClassPublic;
                        }

                    case WellKnownTags.Constant:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.ConstantProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.ConstantPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.ConstantInternal;
                            default:
                                return Glyph.ConstantPublic;
                        }

                    case WellKnownTags.Delegate:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.DelegateProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.DelegatePrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.DelegateInternal;
                            default:
                                return Glyph.DelegatePublic;
                        }

                    case WellKnownTags.Enum:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.EnumProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.EnumPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.EnumInternal;
                            default:
                                return Glyph.EnumPublic;
                        }

                    case WellKnownTags.EnumMember:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.EnumMemberProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.EnumMemberPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.EnumMemberInternal;
                            default:
                                return Glyph.EnumMemberPublic;
                        }

                    case WellKnownTags.Error:
                        return Glyph.Error;

                    case WellKnownTags.Event:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.EventProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.EventPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.EventInternal;
                            default:
                                return Glyph.EventPublic;
                        }

                    case WellKnownTags.ExtensionMethod:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.ExtensionMethodProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.ExtensionMethodPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.ExtensionMethodInternal;
                            default:
                                return Glyph.ExtensionMethodPublic;
                        }

                    case WellKnownTags.Field:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.FieldProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.FieldPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.FieldInternal;
                            default:
                                return Glyph.FieldPublic;
                        }

                    case WellKnownTags.Interface:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.InterfaceProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.InterfacePrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.InterfaceInternal;
                            default:
                                return Glyph.InterfacePublic;
                        }

                    case WellKnownTags.Intrinsic:
                        return Glyph.Intrinsic;

                    case WellKnownTags.Keyword:
                        return Glyph.Keyword;

                    case WellKnownTags.Label:
                        return Glyph.Label;

                    case WellKnownTags.Local:
                        return Glyph.Local;

                    case WellKnownTags.Namespace:
                        return Glyph.Namespace;

                    case WellKnownTags.Method:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.MethodProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.MethodPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.MethodInternal;
                            default:
                                return Glyph.MethodPublic;
                        }

                    case WellKnownTags.Module:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.ModulePublic;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.ModulePrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.ModuleInternal;
                            default:
                                return Glyph.ModulePublic;
                        }

                    case WellKnownTags.Folder:
                        return Glyph.OpenFolder;

                    case WellKnownTags.Operator:
                        return Glyph.Operator;

                    case WellKnownTags.Parameter:
                        return Glyph.Parameter;

                    case WellKnownTags.Property:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.PropertyProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.PropertyPrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.PropertyInternal;
                            default:
                                return Glyph.PropertyPublic;
                        }

                    case WellKnownTags.RangeVariable:
                        return Glyph.RangeVariable;

                    case WellKnownTags.Reference:
                        return Glyph.Reference;

                    case WellKnownTags.NuGet:
                        return Glyph.NuGet;

                    case WellKnownTags.Structure:
                        switch (GetAccessibility(tags))
                        {
                            case Microsoft.CodeAnalysis.Accessibility.Protected:
                                return Glyph.StructureProtected;
                            case Microsoft.CodeAnalysis.Accessibility.Private:
                                return Glyph.StructurePrivate;
                            case Microsoft.CodeAnalysis.Accessibility.Internal:
                                return Glyph.StructureInternal;
                            default:
                                return Glyph.StructurePublic;
                        }

                    case WellKnownTags.TypeParameter:
                        return Glyph.TypeParameter;

                    case WellKnownTags.Snippet:
                        return Glyph.Snippet;

                    case WellKnownTags.Warning:
                        return Glyph.CompletionWarning;

                    case WellKnownTags.StatusInformation:
                        return Glyph.StatusInformation;
                }
            }

            return Glyph.None;
        }

        private static Microsoft.CodeAnalysis.Accessibility GetAccessibility(ImmutableArray<string> tags)
        {
            if (tags.Contains(WellKnownTags.Public))
            {
                return Microsoft.CodeAnalysis.Accessibility.Public;
            }
            if (tags.Contains(WellKnownTags.Protected))
            {
                return Microsoft.CodeAnalysis.Accessibility.Protected;
            }
            if (tags.Contains(WellKnownTags.Internal))
            {
                return Microsoft.CodeAnalysis.Accessibility.Internal;
            }
            if (tags.Contains(WellKnownTags.Private))
            {
                return Microsoft.CodeAnalysis.Accessibility.Private;
            }
            return Microsoft.CodeAnalysis.Accessibility.NotApplicable;
        }
    }
}
