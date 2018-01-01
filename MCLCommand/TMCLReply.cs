namespace TMCLDirect
{
    public sealed class TMCLReply
    {
        #region Ctor

        internal TMCLReply(byte[] buffer)
        {
            Buffer = buffer;
            Checksum = buffer[8];
            byte chksum = 0;
            for (int i = 0; i < 8; i++) chksum += buffer[i];
            Address = buffer[0];
            Status = buffer[2];
            Value = (buffer[4] << 24) | (buffer[5] << 16) | (buffer[6] << 8) | buffer[7];
            ChecksumError = (Status == TMCL.SC_WRONG_CHECKSUM || Checksum != chksum);
        }

        #endregion

        #region Properties
        public byte[] Buffer { get; private set; }
        public byte Checksum { get; private set; }
        public bool ChecksumError { get; private set; }
        public byte Address { get; private set; }
        public byte Status { get; private set; }
        public int Value { get; private set; }

        #endregion

        #region override

        public override string ToString()
        {
            return string.Format("Address: {0}, Status: {1}, Value: {2}, Checksum: {3}, ChecksumError: {4}",
                Address, Status, Value, Checksum, ChecksumError);
        }

        #endregion
    }
}
