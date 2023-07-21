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
using System.Windows.Media;
using Forms = System.Windows; 

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

                using (Transaction t = new Transaction(curDoc))
                {
                    t.Start("Demolish to Phase");

                    foreach (Reference pick in pickList)
                    {
                        // get current element from the pick list
                        Element curElem = curDoc.GetElement(pick);

                        ElementId demoPhaseId;

                        Parameter paramPhaseDemo = curElem.get_Parameter(BuiltInParameter.PHASE_DEMOLISHED);

                        // get the PhaseDemolishedID
                        if (curWin.selectedPhase == "None")
                        {
                            demoPhaseId = ElementId.InvalidElementId;
                        }
                        else if (curWin.selectedPhase != "None")
                        {
                            // get the value of the PHASE_CREATED parameter for curElem
                            Parameter paramPhaseCreated = curElem.get_Parameter(BuiltInParameter.PHASE_CREATED);
                            string paramCreatedPhase = paramPhaseCreated.ToString();

                            // ?? get the index of the PHASE_CREATED value from the phase array
                            int indexCreated = curWin.arrayPhases.get_Item(paramCreatedPhase);

                            // get the value of the selectedDemoPhase variable
                            Phase selectedPhaseDemo= Utils.getPhaseByName(curDoc, curWin.selectedPhase);
                            string paramDemoPhase = paramPhaseDemo.ToString();

                            // get the index of the selectedDemoPhase variable from the phase array
                            int indexDemo = curWin.arrayPhases.get_Item(paramDemoPhase);

                            // if the index of selectedDemoPhase is less than the index of PHASE_CREATED warn the user
                            if (indexDemo < indexCreated)
                            {
                                // 
                                string msgText = "Invalid oder of phases: an object cannot be demolished before it was created";
                                string msgTitle = "Error";
                                Forms.MessageBoxButton msgButtons = Forms.MessageBoxButton.OK;

                                Forms.MessageBox.Show(msgText, msgTitle, msgButtons, Forms.MessageBoxImage.Warning);
                            }
                            // if the index is greater than the index of PHASE_CREATED
                            else if (indexDemo > indexCreated)
                            {
                                // set the demoPhaseID = selectedDemoPhase.Id
                                demoPhaseId = selectedPhaseDemo.Id;
                            }                                                        
                        }

                        // set the Phase_Demolished parameter
                        paramPhaseDemo.Set(demoPhaseId);
                    }

                    t.Commit();
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
