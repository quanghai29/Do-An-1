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
