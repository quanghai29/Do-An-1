using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    // Nhóm String Operation
    public abstract class StringOperation
    {
        public StringArgs Args { get; set; }
        public abstract string Operation(string Origin);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void Config();
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

    // Name
    public class StringName
    {
        public string newName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }
    }

}
