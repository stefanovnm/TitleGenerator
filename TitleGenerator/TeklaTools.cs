using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using Tekla.Structures.Model;
using Tekla.Structures;
using Tekla.Structures.Drawing;


namespace TitleGenerator
{
    public class TeklaTools
    {
        public string GetAssemblyNumbers()
        {
            Model Model = new Model();

            Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            ModelObjectEnumerator Enum = selector.GetSelectedObjects();

            SortedSet<string> list = new SortedSet<string>();

            string current = string.Empty;

            string assemblyNumber = string.Empty;
            string assemblyPhase = string.Empty;
            string assemblySerialNumber = string.Empty;
            string assemblyPrefix = string.Empty;

            Phase phase = new Phase();

            while (Enum.MoveNext())
            {
                Beam beam = Enum.Current as Beam;
                
                if (beam != null)
                {
                    Tekla.Structures.Model.Assembly assembly = beam.GetAssembly() as Tekla.Structures.Model.Assembly;

                    assembly.GetReportProperty("ASSEMBLY_POS", ref assemblyNumber);
                    assembly.GetPhase(out phase);
                    assemblyPhase = phase.PhaseNumber.ToString();
                    //assembly.GetReportProperty("SERIAL_NUMBER", ref assemblySerialNumber);
                    assembly.GetReportProperty("ASSEMBLY_PREFIX", ref assemblyPrefix);

                    var arr = assemblyNumber.Split('/');

                    assemblySerialNumber = arr[1];

                    current = assemblySerialNumber;/* + "_" + assemblyPhase + "_" + assemblyPrefix + "[" + assemblyNumber + "]"*/

                    list.Add(current);
                }

                ContourPlate cPlate = Enum.Current as ContourPlate;

                if (cPlate != null)
                {
                    Tekla.Structures.Model.Assembly assemblyCPL = cPlate.GetAssembly() as Tekla.Structures.Model.Assembly;

                    assemblyCPL.GetReportProperty("ASSEMBLY_POS", ref assemblyNumber);
                    assemblyCPL.GetPhase(out phase);
                    assemblyPhase = phase.PhaseNumber.ToString();
                    //assembly.GetReportProperty("SERIAL_NUMBER", ref assemblySerialNumber);
                    assemblyCPL.GetReportProperty("ASSEMBLY_PREFIX", ref assemblyPrefix);

                    var arr = assemblyNumber.Split('/');

                    assemblySerialNumber = arr[1];

                    current = assemblySerialNumber;/* + "_" + assemblyPhase + "_" + assemblyPrefix + "[" + assemblyNumber + "]"*/

                    list.Add(current);
                }
            }
            string shorten = ReturnShortenName(list);

            string result = "El. " + shorten + "("+assemblyPhase+"_"+assemblyPrefix+")";

            return result;
        }

        public string GetInfo()
        {
            Model Model = new Model();

            Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            ModelObjectEnumerator Enum = selector.GetSelectedObjects();

            return Enum.GetSize().ToString();
        }

        private string ReturnShortenName(SortedSet<string> list)
        {
            int previous = -1;
            bool wasPrevoiusNeeded = true;
            bool isFirst = true;

            int count = 0;
            int index = 0;
            int max = list.Count;

            string result = string.Empty;

            foreach (var el in list)
            {


                if (isFirst)
                {
                    result = el;
                    isFirst = false;
                }
                else
                {
                    if (previous + 1 == Convert.ToInt32(el))
                    {
                        count += 1;

                        if (count > 1 && wasPrevoiusNeeded == true)
                        {
                            result += "...";
                            wasPrevoiusNeeded = false;
                        }

                        if (index == max-1)
                        {
                            if (count == 1)
                            {
                                result += ", " + el;
                            }
                            else
                            {
                                result += el;
                            }
                            
                        }
                    }
                    else
                    {

                        wasPrevoiusNeeded = true;
                        if (count == 1)
                        {
                            count = 0;
                            result += previous.ToString() + ", " + el;
                        }

                        if (count > 1)
                        {
                            count = 0;
                            result +=previous.ToString() + ", " + el;
                        }
                        else
                        {
                            result += ", " + el;
                        }
                    }
                }
                
                previous = Convert.ToInt32(el);

                index += 1;
            }



            return result;
        }

        public string GetActiveMultiInfo()
        {
            string result = string.Empty;

            DrawingHandler drawingHandler = new DrawingHandler();

            Drawing drawing = drawingHandler.GetActiveDrawing();

            string title2 = string.Empty;

            drawing.GetUserProperty("PM_DRAWING_TITLE_2", ref title2);

            return title2;
        }

        public string GetMultiInfo()
        {
            string result = string.Empty;

            DrawingHandler drawingHandler = new DrawingHandler();

            DrawingEnumerator selectedDrawings = drawingHandler.GetDrawingSelector().GetSelected();

            string title2 = string.Empty;

            while (selectedDrawings.MoveNext())
            {
                Drawing drawing = selectedDrawings.Current;
                drawing.GetUserProperty("PM_DRAWING_TITLE_2", ref title2);
            }

            return title2;
        }
    }
}