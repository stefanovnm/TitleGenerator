using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace TitleGenerator
{
    public class TeklaTools
    {
        public string GetAssemblyNumbers()
        {
            Model model = new Model();

            Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            ModelObjectEnumerator enumerator = selector.GetSelectedObjects();

            SortedSet<string> list = new SortedSet<string>();

            string current = string.Empty;

            string assemblyNumber = string.Empty;
            string assemblyPhase = string.Empty;
            string assemblySerialNumber = string.Empty;
            string assemblyPrefix = string.Empty;

            Phase phase = new Phase();

            while (enumerator.MoveNext())
            {
                Beam beam = enumerator.Current as Beam;
                
                if (beam != null)
                {
                    Tekla.Structures.Model.Assembly assembly = beam.GetAssembly() as Tekla.Structures.Model.Assembly;

                    assembly.GetReportProperty("ASSEMBLY_POS", ref assemblyNumber);
                    assembly.GetPhase(out phase);
                    assemblyPhase = phase.PhaseNumber.ToString();
                    assembly.GetReportProperty("ASSEMBLY_PREFIX", ref assemblyPrefix);

                    var arr = assemblyNumber.Split('/');

                    assemblySerialNumber = arr[1];

                    current = assemblySerialNumber;

                    list.Add(current);
                }

                ContourPlate contourPlate = enumerator.Current as ContourPlate;

                if (contourPlate != null)
                {
                    Tekla.Structures.Model.Assembly assemblyCPL = contourPlate.GetAssembly() as Tekla.Structures.Model.Assembly;

                    assemblyCPL.GetReportProperty("ASSEMBLY_POS", ref assemblyNumber);
                    assemblyCPL.GetPhase(out phase);
                    assemblyPhase = phase.PhaseNumber.ToString();
                    assemblyCPL.GetReportProperty("ASSEMBLY_PREFIX", ref assemblyPrefix);

                    var arr = assemblyNumber.Split('/');

                    assemblySerialNumber = arr[1];

                    current = assemblySerialNumber;

                    list.Add(current);
                }
            }

            string shorten = this.ReturnShortenName(list);

            string result = "El. " + shorten + "(" + assemblyPhase + "_" + assemblyPrefix + ")";

            return result;
        }

        public string GetInfo()
        {
            Model model = new Model();

            Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            ModelObjectEnumerator enumerator = selector.GetSelectedObjects();

            return enumerator.GetSize().ToString();
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

            List<DrawingInfo> listOfDrawings = new List<DrawingInfo>();

            string number = string.Empty;
            string projectNumber = string.Empty;
            string title1 = string.Empty;
            string title2 = string.Empty;
            string name = string.Empty;

            while (selectedDrawings.MoveNext())
            {
                var currentDrawingInfo = new DrawingInfo();

                Drawing drawing = selectedDrawings.Current;

                currentDrawingInfo.BaseName = drawing.GetType() + drawing.Mark;

                drawing.GetUserProperty("PM_DRAWING_TITLE_1", ref title1);
                drawing.GetUserProperty("PM_DRAWING_TITLE_2", ref title2);
                drawing.GetUserProperty("Nr_dokumentu", ref number);
                drawing.GetUserProperty("Nr_projektu", ref projectNumber);

                currentDrawingInfo.DrawingTitle1 = title1;
                currentDrawingInfo.DrawingTitle2 = title2;
                currentDrawingInfo.DrawingProjectNumber = projectNumber;
                currentDrawingInfo.DrawingNumber = number;

                listOfDrawings.Add(currentDrawingInfo);

                name = currentDrawingInfo.ToString();
            }
            
            return name;
        }

        public string ReturnFullName()
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

            title2 = title2.Remove(0, 4);

            result = this.ExpandTitle(title2);

            return result;
        }

        public void ReturnFullNameToTextFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string document = "drawings.csv";

            DrawingHandler drawingHandler = new DrawingHandler();

            DrawingEnumerator selectedDrawings = drawingHandler.GetDrawingSelector().GetSelected();

            string result = string.Empty;

            string title1 = string.Empty;
            string title2 = string.Empty;
            string number = string.Empty;
            string projectNumber = string.Empty;

            while (selectedDrawings.MoveNext())
            {
                var currentDrawingInfo = new DrawingInfo();

                Drawing drawing = selectedDrawings.Current;

                currentDrawingInfo.BaseName = drawing.GetType() + drawing.Mark;

                drawing.GetUserProperty("PM_DRAWING_TITLE_1", ref title1);
                drawing.GetUserProperty("PM_DRAWING_TITLE_2", ref title2);
                drawing.GetUserProperty("Nr_dokumentu", ref number);
                drawing.GetUserProperty("Nr_projektu", ref projectNumber);

                currentDrawingInfo.DrawingTitle1 = title1;
                currentDrawingInfo.DrawingTitle2 = title2;
                currentDrawingInfo.DrawingProjectNumber = projectNumber;
                currentDrawingInfo.DrawingNumber = number;
                currentDrawingInfo.DrawingTitle2Full = this.ExpandTitle(title2.Remove(0,4));

                result += currentDrawingInfo.ToString() + "\n";
            }

            var lines = result.Split('\n');

            using (StreamWriter outputFile = new StreamWriter(path + @"\" + document))
            {
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                }
            }
        }

        public void WriteNumber(string number)
        {
            DrawingHandler drawingHandler = new DrawingHandler();

            DrawingEnumerator selectedDrawings = drawingHandler.GetDrawingSelector().GetSelected();

            int num = 300;
            int index = 0;
            while (selectedDrawings.MoveNext())
            {
                Drawing drawing = selectedDrawings.Current;

                drawing.SetUserProperty("Nr_dokumentu", number + (num + index).ToString());
                drawing.CommitChanges();
                index++;
            }
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

                        if (index == max - 1)
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
                            if (index < 3)
                            {
                                result += ", " + previous.ToString() + ", " + el;
                            }
                            else
                            {
                                result += ", " + previous.ToString() + ", " + el;
                            }
                        }

                        if (count > 1)
                        {
                            result += previous.ToString() + ", " + el;
                        }

                        if (count == 0)
                        {
                            result += ", " + el;
                        }

                        count = 0;
                    }
                }

                previous = Convert.ToInt32(el);

                index += 1;
            }

            return result;
        }

        private string ExpandTitle(string input)
        {
            string result = string.Empty;

            var listOfNumbers = input.Split(new char[] { ',', '(', ' ' });

            listOfNumbers = listOfNumbers.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string suffix = listOfNumbers.Last().Remove(listOfNumbers.Last().Length - 1);

            listOfNumbers = listOfNumbers.Take(listOfNumbers.Count() - 1).ToArray();

            foreach (var item in listOfNumbers)
            {
                if (item.Contains("..."))
                {
                    result += this.EnlargeRange(item, suffix) + ", ";
                }
                else
                {
                    result += item + "_" + suffix + ", ";
                }
            }

            result = result.Remove(result.Length - 2);

            return result;
        }

        private string EnlargeRange(string range, string suffix)
        {
            string result = string.Empty;

            var array = range.Split('.');

            array = array.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            int min = Convert.ToInt32(array[0]);
            int max = Convert.ToInt32(array[1]);

            while (min <= max)
            {
                result += min.ToString() + "_" + suffix + ", ";
                min++;
            }

            result = result.Remove(result.Length - 2);

            return result;
        }
    }
}