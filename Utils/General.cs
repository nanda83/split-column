using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Autodesk.Revit.UI;

namespace Utils
{
    public class General
    {
        /* MENSAJE DE INFORMACION */

        public static void ShowInformationMessage(string message)
        {
            TaskDialog taskDialog = new TaskDialog("INFORMATION");
            taskDialog.TitleAutoPrefix = false;
            taskDialog.MainContent = message;
            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
            taskDialog.MainInstruction = "";
            taskDialog.FooterText = "MaFer";

            taskDialog.Show();
        }

        /* MENSAJE DE ADVERTENCIA */

        public static void ShowWarningMessage(string message)
        {
            TaskDialog taskDialog = new TaskDialog("WARNING");
            taskDialog.TitleAutoPrefix = false;
            taskDialog.MainContent = message;
            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            taskDialog.MainInstruction = "";
            taskDialog.FooterText = "MaFer";

            taskDialog.Show();
        }

        /* MENSAJE DE ERROR */
        public static void ShowErrorMessage(string message)
        {
            TaskDialog taskDialog = new TaskDialog("ERROR");
            taskDialog.TitleAutoPrefix = false;
            taskDialog.MainContent = message;
            taskDialog.MainIcon = TaskDialogIcon.TaskDialogIconError;
            taskDialog.MainInstruction = "";
            taskDialog.FooterText = "MaFer";

            taskDialog.Show();
        }
    }
}