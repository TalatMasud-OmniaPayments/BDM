using Omnia.PIE.VTA.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.ViewModels
{
    public class DevicesViewModel : BaseViewModel
    {
        private ObservableCollection<Cassette> _CashCassettes;
        public ObservableCollection<Cassette> CashCassettes
        {
            get
            {
                if (_CashCassettes == null)
                    _CashCassettes = new ObservableCollection<Cassette>();
                return _CashCassettes;
            }
            set
            {
                _CashCassettes = value;
                OnPropertyChanged(() => CashCassettes);
            }
        }

        private CallDuration _CallDuration;
        public CallDuration CallDuration
        {
            get
            {
                if (_CallDuration == null)
                {
                    _CallDuration = new CallDuration();
                }

                return _CallDuration;
            }
            set
            {
                if (value != _CallDuration)
                {
                    _CallDuration = value;
                    OnPropertyChanged(() => CallDuration);
                }
            }
        }

        private Device _CashDispenser;
        public Device CashDispenser
        {
            get
            {
                if (_CashDispenser == null)
                    _CashDispenser = new Device();
                return _CashDispenser;
            }
            set
            {
                if (value != _CashDispenser)
                {
                    _CashDispenser = value;
                    OnPropertyChanged(() => CashDispenser);
                }
            }
        }

        private Device _CardReader;
        public Device CardReader
        {
            get
            {
                if (_CardReader == null)
                {
                    _CardReader = new Device();
                }

                return _CardReader;
            }
            set
            {
                if (value != _CardReader)
                {
                    _CardReader = value;
                    OnPropertyChanged(() => CardReader);
                }
            }
        }

        private Device _Scanner;
        public Device Scanner
        {
            get
            {
                if (_Scanner == null)
                    _Scanner = new Device();

                return _Scanner;
            }
            set
            {
                if (value != _Scanner)
                {
                    _Scanner = value;
                    OnPropertyChanged(() => Scanner);
                }
            }
        }

        private Device _ReceiptPrinter;
        public Device ReceiptPrinter
        {
            get
            {
                if (_ReceiptPrinter == null)
                    _ReceiptPrinter = new Device();
                return _ReceiptPrinter;
            }
            set
            {
                if (value != _ReceiptPrinter)
                {
                    _ReceiptPrinter = value;
                    OnPropertyChanged(() => ReceiptPrinter);
                }
            }
        }

        private Device _StatementPrinter;
        public Device StatementPrinter
        {
            get
            {
                if (_StatementPrinter == null)
                    _StatementPrinter = new Device();
                return _StatementPrinter;
            }
            set
            {
                if (value != _StatementPrinter)
                {
                    _StatementPrinter = value;
                    OnPropertyChanged(() => StatementPrinter);
                }
            }
        }

        private Device _PinPad;
        public Device PinPad
        {
            get
            {
                if (_PinPad == null)
                    _PinPad = new Device();
                return _PinPad;
            }
            set
            {
                if (value != _PinPad)
                {
                    _PinPad = value;
                    OnPropertyChanged(() => PinPad);
                }
            }
        }

        private Device _Doors;
        public Device Doors
        {
            get
            {
                if (_Doors == null)
                    _Doors = new Device();
                return _Doors;
            }
            set
            {
                if (value != _Doors)
                {
                    _Doors = value;
                    OnPropertyChanged(() => Doors);
                }
            }
        }

        private Device _Sensors;
        public Device Sensors
        {
            get
            {
                if (_Sensors == null)
                    _Sensors = new Device();
                return _Sensors;
            }
            set
            {
                if (value != _Sensors)
                {
                    _Sensors = value;
                    OnPropertyChanged(() => Sensors);
                }
            }
        }

        private Device _Camera;
        public Device Camera
        {
            get
            {
                if (_Camera == null)
                    _Camera = new Device();

                return _Camera;
            }
            set
            {
                if (value != _Camera)
                {
                    _Camera = value;
                    OnPropertyChanged(() => Camera);
                }
            }
        }

        private Device _RFIDReader;
        public Device RFIDReader
        {
            get
            {
                if (_RFIDReader == null)
                    _RFIDReader = new Device();
                return _RFIDReader;
            }
            set
            {
                if (value != _RFIDReader)
                {
                    _RFIDReader = value;
                    OnPropertyChanged(() => RFIDReader);
                }
            }
        }

        private Device _Auxiliaries;
        public Device Auxiliaries
        {
            get
            {
                if (_Auxiliaries == null)
                    _Auxiliaries = new Device();
                return _Auxiliaries;
            }
            set
            {
                if (value != _Auxiliaries)
                {
                    _Auxiliaries = value;
                    OnPropertyChanged(() => Auxiliaries);
                }
            }
        }

        private Device _SignPad;
        public Device SignPad
        {
            get
            {
                if (_SignPad == null)
                    _SignPad = new Device();
                return _SignPad;
            }
            set
            {
                if (value != _SignPad)
                {
                    _SignPad = value;
                    OnPropertyChanged(() => SignPad);
                }
            }
        }

        private Device _Indicators;
        public Device Indicators
        {
            get
            {
                if (_Indicators == null)
                    _Indicators = new Device();
                return _Indicators;
            }
            set
            {
                if (value != _Indicators)
                {
                    _Indicators = value;
                    OnPropertyChanged(() => Indicators);
                }
            }
        }

        private Device _DVCSignal;
        public Device DVCSignal
        {
            get
            {
                if (_DVCSignal == null)
                    _DVCSignal = new Device();

                return _DVCSignal;
            }
            set
            {
                if (value != _DVCSignal)
                {
                    _DVCSignal = value;
                    OnPropertyChanged(() => DVCSignal);
                }
            }
        }

        private Device _VDM;
        public Device VDM
        {
            get
            {
                if (_VDM == null)
                    _VDM = new Device();

                return _VDM;
            }
            set
            {
                if (value != _VDM)
                {
                    _VDM = value;
                    OnPropertyChanged(() => VDM);
                }
            }
        }

        private Device _VFD;
        public Device VFD
        {
            get
            {
                if (_VFD == null)
                    _VFD = new Device();

                return _VFD;
            }
            set
            {
                if (value != _VFD)
                {
                    _VFD = value;
                    OnPropertyChanged(() => VFD);
                }
            }
        }

        private Device _TMD;
        public Device TMD
        {
            get
            {
                if (_TMD == null)
                    _TMD = new Device();

                return _TMD;
            }
            set
            {
                if (value != _TMD)
                {
                    _TMD = value;
                    OnPropertyChanged(() => TMD);
                }
            }
        }

        private Device _ChequeScanner;
        public Device ChequeScanner
        {
            get
            {
                if (_ChequeScanner == null)
                    _ChequeScanner = new Device();

                return _ChequeScanner;
            }
            set
            {
                if (value != _ChequeScanner)
                {
                    _ChequeScanner = value;
                    OnPropertyChanged(() => ChequeScanner);
                }
            }
        }

        private Device _JournalPrinter;
        public Device JournalPrinter
        {
            get
            {
                if (_JournalPrinter == null)
                    _JournalPrinter = new Device();

                return _JournalPrinter;
            }
            set
            {
                if (value != _JournalPrinter)
                {
                    _JournalPrinter = value;
                    OnPropertyChanged(() => JournalPrinter);
                }
            }
        }

        private Device _CashAcceptor;
        public Device CashAcceptor
        {
            get
            {
                if (_CashAcceptor == null)
                    _CashAcceptor = new Device();

                return _CashAcceptor;
            }
            set
            {
                if (value != _CashAcceptor)
                {
                    _CashAcceptor = value;
                    OnPropertyChanged(() => CashAcceptor);
                }
            }
        }

        private Device _IDScanner;
        public Device IDScanner
        {
            get
            {
                if (_IDScanner == null)
                    _IDScanner = new Device();

                return _IDScanner;
            }
            set
            {
                if (value != _IDScanner)
                {
                    _IDScanner = value;
                    OnPropertyChanged(() => IDScanner);
                }
            }
        }

        private Device _A4Printer;
        public Device A4Printer
        {
            get
            {
                if (_A4Printer == null)
                    _A4Printer = new Device();

                return _A4Printer;
            }
            set
            {
                if (value != _A4Printer)
                {
                    _A4Printer = value;
                    OnPropertyChanged(() => A4Printer);
                }
            }
        }

        private Device _A4Scanner;
        public Device A4Scanner
        {
            get
            {
                if (_A4Scanner == null)
                    _A4Scanner = new Device();

                return _A4Scanner;
            }
            set
            {
                if (value != _A4Scanner)
                {
                    _A4Scanner = value;
                    OnPropertyChanged(() => A4Scanner);
                }
            }
        }
    }
}
