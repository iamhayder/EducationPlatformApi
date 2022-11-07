namespace EducationPlatformApi.Helpers
{
    public class StatusType
    {
        private StatusType(string value) { Value = value; }
        public string Value { get; private set; }
        public static StatusType Success => new("success");
        public static StatusType Error => new("error");
    }
}
