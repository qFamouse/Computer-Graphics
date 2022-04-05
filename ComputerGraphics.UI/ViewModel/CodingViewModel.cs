using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils.Coding;
using ComputerGraphics.UI.Utils.Coding.LZW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics.UI.ViewModel
{
    internal class CodingViewModel : BaseViewModel
    {
        private string _inputData;
        private string _outputData;

        public string InputData
        {
            get => _inputData;
            set
            {
                _inputData = value;
                OnPropertyChanged(nameof(InputData));
            }
        }
        public string OutputData
        {
            get => _outputData;
            set
            {
                _outputData = value;
                OnPropertyChanged(nameof(OutputData));
            }
        }

        #region Codings

        private Coding _currentCoding = new Coding(null);

        private LzwCoding _lzwCoding = new LzwCoding(new LzwDictionary());

        #endregion

        public CodingViewModel() { }

        public RelayCommand SwitchToLzw
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    _currentCoding.CodingAlgorithm = _lzwCoding;
                });
            }
        }


        public RelayCommand EncodeCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        OutputData = String.Join(" ", _currentCoding.Encode(InputData));
                    }
                    catch (NullReferenceException e)
                    {
                        ShowErrorMessageBox("Please, choose the algorithm!");
                    }
                    catch (Exception e)
                    {
                        ShowErrorMessageBox(e.Message);
                    }

                });
            }
        }

        public RelayCommand DecodeCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        byte[] encodedCharacters = InputData.Split(' ').Select(s => Byte.Parse(s)).ToArray();
                        OutputData = _currentCoding.Decode(encodedCharacters);
                    }
                    catch (NullReferenceException e)
                    {
                        ShowErrorMessageBox("Please, choose the algorithm!");
                    }
                    catch (Exception e)
                    {
                        ShowErrorMessageBox(e.Message);
                    }
                });
            }
        }

        private void ShowErrorMessageBox(string error)
        {
            MessageBox_Show(null, error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
