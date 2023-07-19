using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchDemolish
{
    internal static class Utils
    {
        public static bool ShowForm;
        internal static void Run(UIApplication uiapp)
        {
            frmBatchDemolish curWin = new frmBatchDemolish(uiapp.ActiveUIDocument.Document);
            curWin.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            curWin.ShowDialog();

            if (Utils.ShowForm)
            {
                IList<Reference> pickList = uiapp.ActiveUIDocument.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // check if the element has a "Phase Demolished" parameter
                Parameter paramPhaseDemo = pickList.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED);

                if (paramPhaseDemo != null)
                {
                    // set the value of "Phase Demolished" to "None"
                    paramPhaseDemo.Set(ElementId.InvalidElementId);
                }                
            }
        }

        #region Ribbon

        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        #endregion
    }
}
