using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

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

    //NewCase
    public class NewCaseArgs : StringArgs,INotifyPropertyChanged
    {
        //Kiểu NewCase;
        public string Option { get; set; }

        private void NotifyChange(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FullnameNormalize : StringArgs, INotifyPropertyChanged
    {
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
            if (Origin == null)
                return null;
            var args = Args as ReplaceArgs;
            var from = args.From;
            var to = args.To;
            return Origin.Replace(from, to);
        }
    }

    //NewCase 
    public class NewCaseOperation : StringOperation, INotifyPropertyChanged
    {
        public override string Name => "New Case";

        public override string Description
        {
            get
            {
                var args = Args as NewCaseArgs;
                if (args.Option == "")
                {
                    return "heLlO/HELLO/Hello/hello";
                }
                else
                {
                    return args.Option;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringOperation Clone()
        {
            var oldArgs = Args as NewCaseArgs;
            return new NewCaseOperation()
            {
                Args = new NewCaseArgs()
                {
                    Option = oldArgs.Option
                }
            };
        }

        public override void Config()
        {
            var screen = new NewCaseConfigDialog(Args);
            if (screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("Description"));
            }
        }

        public override string Operation(string Origin)
        {
            var args = Args as NewCaseArgs;
            var option = args.Option;
            var result = "";
            if (option.Contains("Upper"))
            {
                result = Origin.ToUpper();
            }
            else if (option.Contains("Lower"))
            {
                result = Origin.ToLower();
            }
            else if (option.Contains("Cammel"))
            {
                var temp = new Support();
                result = Origin.ToLower();
                result = "_" + result;
                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (result[i] < 97 || result[i] > 122)
                    {
                        if (result[i + 1] >= 97 && result[i + 1] <= 122)
                        {
                            result = temp.MyReplace(result, i + 1);
                            i++;
                        }
                    }
                }
                result = result.Substring(1, result.Length - 1);
            }

            return result;
        }


    }

    //
    public class Support
    {
        /// <summary>
        /// Upper Case một kí tự tại vị trí cho trước trong một chuỗi
        /// </summary>
        /// <param name="origin">Chuỗi gốc</param>
        /// <param name="foundPositon">index của kí tự cần được upper case</param>
        /// <returns>Chuỗi mới</returns>
        public string MyReplace(string origin, int foundPositon)
        {
            string result = "";
            string temp = "";

            temp = origin[foundPositon].ToString().ToUpper();

            result += origin.Substring(0, foundPositon);
            result += temp;
            result += origin.Substring(foundPositon + 1, origin.Length - foundPositon - 1);

            return result;
        }
    }


    //FullnameNormalize
    public class FullnameNormalizeOperation : StringOperation, INotifyPropertyChanged
    {
        public override string Name => "Fullname Normalize";

        public override string Description => "This Is The Fullname Normalize method";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringOperation Clone()
        {
            return new FullnameNormalizeOperation();
        }

        public override void Config()
        {

        }

        public override string Operation(string Origin)
        {
            var result = Origin.Trim();//Xóa khoảng trắng ở đầu và cuối chuỗi

            //Xử lí khoảng trắng giữa các từ và kí tự đầu mỗi từ
            result = result.ToLower();
            string[] arrWord = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            result = "";
            var temp = new Support();
            string word;
            for (int i = 0; i < arrWord.Length; i++)
            {
                word = arrWord[i].ToString();
                word = temp.MyReplace(word, 0);
                word += " ";
                result += word;
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }

    public class ISBNOperation: StringOperation, INotifyPropertyChanged
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
            Regex regex = new Regex(@"/[0-9^-]{13}/");
            if (isbnArgs.Direction == "before")
            {
                if (ext != "" && regex.IsMatch(Origin) == true)
                {
                    string isbn = Origin.Substring(Origin.Length - ext.Length - 13, 13);
                    string name = Origin.Substring(0, Origin.Length - isbn.Length - ext.Length);
                    newName = $"{isbn}{name}{ext}";
                }
                else
                {
                    // khi file khong xac dinh hoac la folder
                    return null;
                }
            }
            else if (isbnArgs.Direction == "after")
            {
                if (ext != "" && regex.IsMatch(Origin) == true)
                {
                    string isbn = Origin.Substring(0, 13);
                    string name = Origin.Substring(13, Origin.Length - isbn.Length - ext.Length);
                    newName = $"{name}{isbn}{ext}";
                }
                else
                {
                    // khi file khong xac dinh hoac la folder
                    return null;
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
