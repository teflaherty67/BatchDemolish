using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BatchDemolish
{
    internal static class Utils
    {
        public static bool ShowForm;
        internal static void Run(UIApplication uiapp, Document curDoc)
        {
            frmBatchDemolish curWin = new frmBatchDemolish(uiapp.ActiveUIDocument.Document);
            curWin.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            curWin.ShowDialog();

            if (Utils.ShowForm)
            {
                IList<Reference> pickList = uiapp.ActiveUIDocument.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);

                foreach (Reference pick in pickList)
                {
                    Element curElem = curDoc.GetElement(pick);

                    ElementId curElemId = curElem.Id;

                    Phase selectedDemoPhase = Utils.getPhaseByName(curDoc, curWin.selectedPhase);

                    ElementId selectedPhaseId = selectedDemoPhase.Id;

                    Parameter paramPhaseDemo = curElem.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED);

                    if (paramPhaseDemo != null)
                    {
                        paramPhaseDemo.Set(ElementId.selectedPhaseId);
                    }
                }               
            }
        }

        #region Phases

        public static List<Element> getAllPhases(Document curDoc)
        {
            FilteredElementCollector curColl = new FilteredElementCollector(curDoc);
            curColl.OfCategory(BuiltInCategory.OST_Phases);

            return curColl.OrderBy(x => x.Name).ToList();
        }

        public static List<Element> getAllPhaseFilters(Document curDoc)
        {
            FilteredElementCollector curColl = new FilteredElementCollector(curDoc);
            curColl.OfClass(typeof(PhaseFilter));

            return curColl.OrderBy(x => x.Name).ToList();
        }

        public static Phase getPhaseByName(Document curDoc, string phaseName)
        {
            // get all phases
            List<Element> phaseList = getAllPhases(curDoc);

            foreach (Phase curPhase in phaseList)
            {
                if (curPhase.Name == phaseName)
                    return curPhase as Phase;
            }

            return null;

        }

        public static PhaseFilter getPhaseFilterByName(Document curDoc, string phaseFilterName)
        {
            // get all phase filters
            List<Element> phaseFilterList = getAllPhaseFilters(curDoc);

            foreach (PhaseFilter curPhaseFilter in phaseFilterList)
            {
                if (curPhaseFilter.Name == phaseFilterName)
                {
                    return curPhaseFilter as PhaseFilter;
                }
            }

            return null;
        }

        #endregion

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
