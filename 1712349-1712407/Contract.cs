using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace _1712349_1712407
{

    // Danh sách các chức năng
    // interface class
    public class StringArgs
    {

    }

    //Nhóm class args
    //Replace 
    public class ReplaceArgs: StringArgs, INotifyPropertyChanged
    {
        // Thay thế từ có trong chuỗi "From" thành "To"
        public string From { get; set; }
        public string To { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }

    //Move ISBN
    public class ISBNArgs : StringArgs, INotifyPropertyChanged
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

        public abstract StringOperation Clone();
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

        public override StringOperation Clone()
        {
            var oldArgs = Args as ReplaceArgs;
            return new ReplaceOperation()
            {
                Args = new ReplaceArgs()
                {
                    From = oldArgs.From,
                    To = oldArgs.To
                }
            };
        }

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

        public override StringOperation Clone()
        {
            var oldArgs = Args as ISBNArgs;
            return new ISBNOperation()
            {
                Args = new ISBNArgs()
                {
                    Direction = oldArgs.Direction
                }
            };
        }

        public override void Config()
        {
            var args = Args as ISBNArgs;
            var screen = new ISBNConfigDialog(args);
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
                if (ext != "")
                {
                    string isbn = Origin.Substring(Origin.Length - ext.Length - 13, 13);
                    string name = Origin.Substring(0, Origin.Length - isbn.Length - ext.Length);
                    newName = $"{isbn}{name}{ext}";
                }
                else
                {
                    // khi file khong xac dinh hoac la folder
                }
            }
            else if (isbnArgs.Direction == "after")
            {
                if (ext != "")
                {
                    string isbn = Origin.Substring(0, 13);
                    string name = Origin.Substring(13, Origin.Length - isbn.Length - ext.Length);
                    newName = $"{name}{isbn}{ext}";
                }
                else
                {
                    // khi file khong xac dinh hoac la folder
                }
            }
            return Origin.Replace(Origin, newName);
        }
    }


    public class UniqueOperation : StringOperation
    {
        public override string Name => "Unique name";
        public override string Description
        {
            get
            {
                return "Generate unique name";
            }
        }

        public override StringOperation Clone()
        {
            return new UniqueOperation();
        }

        public override void Config()
        {
            throw new NotImplementedException();
        }

        public override string Operation(string Origin)
        {
            string ext = Path.GetExtension(Origin);
            var g = Guid.NewGuid();
            string newName = $"{g.ToString()}{ext}";
            return Origin.Replace(Origin, newName);
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

    public class HistoryAction
    {
        public BindingList<StringOperation> action { get; set; }
        public string actionName { get; set; }
    }
}
