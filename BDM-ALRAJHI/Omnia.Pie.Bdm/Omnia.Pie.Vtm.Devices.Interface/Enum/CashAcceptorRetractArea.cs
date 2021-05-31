namespace Omnia.Pie.Vtm.Devices.Interface
{
	public enum CashAcceptorRetractArea
	{
		Retract = 0,
		Transport = 1,
		Stacker = 2,
		BillCassette = 3,
		Reject = 32
	}

    public enum CashAcceptorCassetteStatus
    {
        OK = 0,
        HIGH = 1,
        FULL = 2,
        FATAL = 3,
        MISSING = 4,
        UNKNOWN
    }
    public enum CashDispenserCassetteStatus
    {
        OK = 0,
        FULL = 1,
        HIGH = 2,
        LOW = 3,
        EMPTY = 4,
        MISSING = 5,
        FATAL = 6,
        NOVALUES = 7,
        NOREFERENCE = 8,
        MANIP
    }
    public enum CashAcceptorStackerStatus
    {
        EMPTY = 0,
        NOTEMPTY = 1,
        FULL = 2,
        NOTSUPP = 3,
        UNKNOWN
    }
    public class CassetteStatus
    {
        public int unitId;
        public string status;
    }
}