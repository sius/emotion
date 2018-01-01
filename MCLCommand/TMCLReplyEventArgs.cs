using System;

namespace TMCLDirect
{
    public class TMCLReplyEventArgs : EventArgs
    {
        internal TMCLReplyEventArgs(TMCLReply reply)
        {
            Reply = reply;
        }
        public TMCLReply Reply { get; private set; }
    }
}
