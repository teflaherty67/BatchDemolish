#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace BatchDemolish
{
    [Transaction(TransactionMode.Manual)]
    public class cmdBatchDemolish : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document curDoc = uidoc.Document;

            // put any code needed for the form here

            // get all the phases in the project
            FilteredElementCollector colPhases = new FilteredElementCollector(curDoc)
                .OfClass(typeof(Phase));

            // create an empty list to hold the phase names
            List<string> phases = new List<string>();

            // loop through the phases
            foreach (Phase curPhase in colPhases)
            {
                // add the phase name to the list
                phases.Add(curPhase.Name);
            }            

            // open form
            Utils.ShowForm = true;
            while (Utils.ShowForm == true)
            {
                Utils.Run(uiapp);
            }

            // get form data and do something

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
