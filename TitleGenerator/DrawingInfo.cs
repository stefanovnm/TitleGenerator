namespace TitleGenerator
{
    public class DrawingInfo
    {
        private string drawingBaseName;
        private string drawingNumber;
        private string drawingProjectNumber;
        private string drawingTitle1;
        private string drawingTitle2;
        private string drawingName;
        private string drawingTitle2Expanded;

        public DrawingInfo()
        {
            this.BaseName = string.Empty;
            this.DrawingNumber = string.Empty;
            this.DrawingProjectNumber = string.Empty;
            this.DrawingTitle1 = string.Empty;
            this.DrawingTitle2 = string.Empty;
            this.DrawingName = string.Empty;
        }

        public string BaseName
        {
            get
            {
                return this.drawingBaseName;
            }

            set
            {
                this.drawingBaseName = value;
            }
        }

        public string DrawingNumber
        {
            get
            {
                return this.drawingNumber;
            }

            set
            {
                this.drawingNumber = value;
            }
        }

        public string DrawingProjectNumber
        {
            get
            {
                return this.drawingProjectNumber;
            }

            set
            {
                this.drawingProjectNumber = value;
            }
        }

        public string DrawingTitle1
        {
            get
            {
                return this.drawingTitle1;
            }

            set
            {
                this.drawingTitle1 = value;
            }
        }

        public string DrawingTitle2
        {
            get
            {
                return this.drawingTitle2;
            }

            set
            {
                this.drawingTitle2 = value;
            }
        }

        public string DrawingTitle2Expanded
        {
            get
            {
                return this.drawingTitle2Expanded;
            }

            set
            {
                this.drawingTitle2Expanded = value;
            }
        }

        public string DrawingName
        {
            get
            {
                return this.drawingName;
            }

            set
            {
                this.drawingName = value;
            }
        }

        public override string ToString()
        {
            return this.BaseName + ";" + this.DrawingName + ";" + this.DrawingNumber + ";" + this.DrawingTitle2 + ";" + this.drawingTitle2Expanded;
            // return this.BaseName + ";" + this.DrawingNumber + ";" + this.DrawingProjectNumber + ";" + this.DrawingTitle1 + ";" + this.DrawingTitle2 + ";" + this.DrawingName;
        }
    }
}
