using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace _1712349_1712407
{

    // Danh sách các chức năng
    // interface class
    public class StringArgs
    {

    }

    //Nhóm class args
    //Replace 
    public class ReplaceArgs: StringArgs
    {
        // Thay thế từ có trong chuỗi "From" thành "To"
        public string From { get; set; }
        public string To { get; set; }
    }

    public class ISBNArgs : StringArgs ,INotifyPropertyChanged
    {
        private string _direction;  // before/after

        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                //NotifyChange("Direction");
                NotifyChange("Description");
            }
        }


           public event PropertyChangedEventHandler PropertyChanged;
           public void NotifyChange(string v)
           {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
           }
    }




    // Nhóm String Operation
    public abstract class StringOperation
    {
        public StringArgs Args { get; set; }
        public abstract string Operation(string Origin);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void Config();
    }

    public class ISBNOperation : StringOperation, INotifyPropertyChanged
    {
        public override string Name => "Move ISBN";
        public override string Description
        {
            get
            {
                var args = Args as ISBNArgs;
                return $"Move ISBN '{args.Direction}' name";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public override void Config()
        {
            var args = Args as ISBNArgs;
            var screen = new ISBNDialog(args);
            if (screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Description"));
            }
        }

        public override string Operation(string Origin)
        {  
            var isbnArgs = Args as ISBNArgs;
            string ext = Path.GetExtension(Origin);
            string newName = null;
            if (isbnArgs.Direction == "before")
            {
                string isbn = Origin.Substring(Origin.Length - ext.Length - 13, 13);
                string name = Origin.Substring(0, Origin.Length - isbn.Length - ext.Length);
                newName = $"{isbn}{name}{ext}";
            }
            else if (isbnArgs.Direction == "after")
            {
                string isbn = Origin.Substring(0, 13);
                string name = Origin.Substring(13, Origin.Length - isbn.Length - ext.Length);
                newName = $"{name}{isbn}{ext}";
            }
            return Origin.Replace(Origin, newName);
        }
    }

    //Nhóm Operation
    public class ReplaceOperation : StringOperation, INotifyPropertyChanged
    {
        public override string Name => "Replace";

        public override string Description
        {
            get
            {
                var args = Args as ReplaceArgs;
                return $"replace '{args.From}' by '{args.To}'";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override void Config()
        {
            var screen = new ReplaceConfigDialog(Args);
            if (screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Description"));
            }
        }

        public override string Operation(string Origin)
        {
            var args = Args as ReplaceArgs;
            var from = args.From;
            var to = args.To;
            return Origin.Replace(from, to);
        }
    }




    public abstract class StringName
    {
        public string newName { get; set; }
        public string Error { get; set; }
    }
    // Name
    public class StringFileName:StringName
    {
        public FileInfo infoName { get; set; }
       
    }
    public class StringFolderName:StringName
    {
        public DirectoryInfo dri { get; set; }
    }

}
