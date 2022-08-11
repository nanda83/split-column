using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

using Autodesk.Revit.UI.Selection;


using Utils;

namespace SplitColumnMF
{
    [Transaction(TransactionMode.Manual)]
  
    public class CmdSplitColumn : IExternalCommand
    {
       public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Reference refColumn = uiDoc.Selection.PickObject(ObjectType.Element, "Selecciona una Columna");
            Element structuralColumn = doc.GetElement(refColumn);
          

            if ((BuiltInCategory)structuralColumn.Category.Id.IntegerValue != BuiltInCategory.OST_StructuralColumns)
            {
                General.ShowInformationMessage("Debe seleccionar una columna");
                return Result.Cancelled;
            }
                        
            Element typeSC = doc.GetElement(structuralColumn.GetTypeId());

            General.ShowInformationMessage("La Columna seleccionada es de tipo " + typeSC.Name);

             
            ICollection<ElementId> intersectedElements = JoinGeometryUtils.GetJoinedElements(doc, structuralColumn);

            List<Element> structuralFramingIntersected = new List<Element>();

            foreach (ElementId intersectedElement in intersectedElements)
            {
                Element iel = doc.GetElement(intersectedElement);
                if (iel.Category.Name == "Structural Framing")
                {
                    structuralFramingIntersected.Add(iel);

                }

            }
            

            double maxHeight = 0;
            double actualHeight = 0;

            foreach (Element sFI in structuralFramingIntersected)
            {

                try
                {
                    ElementType type = doc.GetElement(sFI.GetTypeId()) as ElementType;
                    Parameter h = type.LookupParameter("h");
                    actualHeight = h.AsDouble()*0.3048;
                    if (maxHeight < actualHeight)
                    {
                        maxHeight = actualHeight;
                    }
                }
                catch (Exception error)
                {
                    General.ShowErrorMessage(error.Message);
                }
            }
            General.ShowInformationMessage("La atura Maxima de las vigas es: " + maxHeight + " metros");

            Parameter structuralColumnHeight = structuralColumn.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM);
            double sCHeight = structuralColumnHeight.AsDouble() * 0.3048;
            double splitC = 1 - (maxHeight / sCHeight);

            FamilyInstance sColumnFI = structuralColumn as FamilyInstance;
            Transaction transaction = new Transaction(doc);
            transaction.Start("Corte de Columna");
            
            try
            {
                if (sColumnFI.CanSplit)
                {
                    
                    sColumnFI.Split(splitC);
                    //Como obtengo el nuevo elemento creado con el corte para cambiarle el color!
                    /*podrías ayudarme a separar la rutina por clases, porque cuando hacia return
                     en esas clases me daban errores para usarlos despues, y con el manejo del documento*/
                                        
                }
            }
            catch (Exception error1)
            {
                General.ShowErrorMessage(error1.Message);
            }
            transaction.Commit();
            return Result.Succeeded;
        }
      
    }
    
}