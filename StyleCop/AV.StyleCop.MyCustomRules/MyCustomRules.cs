using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StyleCop;
using StyleCop.CSharp;

namespace AV.StyleCop.MyCustomRules
{
    /// <summary>
    /// Custom analyzer for demo purposes.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class MyCustomRules : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");
            CsDocument doc = (CsDocument)document;
            if ((doc.RootElement != null) && !doc.RootElement.Generated)
            {
                //ProcessElement(doc.RootElement);

                // check all class entries
                //doc.WalkDocument(CheckClasses);

                //doc.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement), null, null);

                doc.WalkDocument(new CodeWalkerElementVisitor<object>(this.ReviewClassFiles), null, null);
            }
        }

        private bool ReviewClassFiles(CsElement element, CsElement parentElement, object context)
        {
            // if current element is not a class then continue walking
            if (element.ElementType == ElementType.Class && !element.Generated)
            {
                Class classElement = (Class)element;

                if (!string.IsNullOrEmpty(classElement.Declaration.Name) && classElement.Declaration.Name.Length > 2)
                {
                    char[] classNameArray = classElement.Declaration.Name.ToCharArray();
                    for (int i = 0; i <= classNameArray.Length - 3; i++)
                    {
                        if (classNameArray[i] == classNameArray[i + 1] && classNameArray[i] == classNameArray[i + 2])
                        {
                            AddViolation(classElement, classElement.Location, "AvoidToRepeatSameLetterMoreThanTwoTimesConsecutively", classElement.FriendlyTypeText);
                        }
                    }
                }
            }
            else if (element.ElementType == ElementType.Field && !element.Generated && element.ActualAccess != AccessModifierType.Public && element.ActualAccess != AccessModifierType.Internal)
            {
                //if (!string.IsNullOrEmpty(element.Declaration.Name) && element.Declaration.Name.Length > 1 && !element.Declaration.Name.StartsWith("_", StringComparison.Ordinal) || element.Declaration.Name.Substring(1, 1).ToLower() != element.Declaration.Name.Substring(1, 1))
                if (!string.IsNullOrEmpty(element.Declaration.Name) && element.Declaration.Name.Length > 1 && (element.Declaration.Name.Substring(0, 1).ToLower() != element.Declaration.Name.Substring(0, 1) || element.Declaration.Name.StartsWith("_", StringComparison.Ordinal) && element.Declaration.Name.Substring(1, 1).ToLower() != element.Declaration.Name.Substring(1, 1)))
                {
                    //AddViolation(element, "PrivateFieldNameMustStartEitherWithUnderscoreFollowedByLowerCaseOrLowerCase", new object[0]);
                    AddViolation(element, element.Location, "PrivateFieldNameMustStartEitherWithUnderscoreFollowedByLowerCaseOrLowerCase", element.FriendlyTypeText);
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether specified element conforms custom rule CR0001.
        /// </summary>
        /// <param name="element">Child element</param>
        /// <param name="parentElement">Parent element</param>
        /// <param name="context">context of element</param>
        /// <returns>true false</returns>
        private bool CheckClasses(CsElement element, CsElement parentElement, object context)
        {
            // if current element is not a class then continue walking
            if (element.ElementType != ElementType.Class)
            {
                return true;
            }

            // check whether class name contains "aaa" letter
            Class classElement = (Class)element;

            if (classElement.Declaration.Name.Contains("aaa") || classElement.Declaration.Name.Contains("AAA"))
            {
                // add violation
                // (note how custom message arguments could be used)
                AddViolation(classElement, classElement.Location, "AvoidUsingAAAInClassNames", classElement.FriendlyTypeText);
            }

            // continue walking in order to find all classes in file
            return true;
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            // Flag a violation if the instance variables are not prefixed with an underscore.
            if (!element.Generated && element.ElementType == ElementType.Field && element.ActualAccess != AccessModifierType.Public &&
                element.ActualAccess != AccessModifierType.Internal && element.Declaration.Name.ToCharArray()[0] != '_')
            {
                AddViolation(element, "InstanceVariablesUnderscorePrefix");
            }
            return true;
        }

        private bool ProcessElement(CsElement element)
        {
            if (base.Cancel)
            {
                return false;
            }

            foreach (CsElement child in element.ChildElements)
            {
                if (!this.ProcessElement(child))
                {
                    return false;
                }
            }

            if (element.ElementType == ElementType.Field && !element.Generated)
            {
                if (!element.Declaration.Name.StartsWith("_", StringComparison.Ordinal) || element.Declaration.Name.Substring(1, 1).ToLower() != element.Declaration.Name.Substring(1, 1))
                {
                    base.AddViolation(element, "PrivateFieldNameMustStartWithUnderscoreFollowedByLowerCase", new object[0]);
                }
            }

            return true;
        }
    }
}
