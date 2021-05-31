namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class Door
	{
		public Door(string id)
		{
			Id = id;
		}

		public string Id { get; private set; }
		public DoorStatus Status { get; set; }
	}
}