using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendme
{
    public class Arguments
    {
        private int mChannel = 0;
        private String mPath = "";
        public int GetChannel() { return mChannel; }
        public void SetChannel (int channel) { mChannel = channel; }
        public string GetPath() { return mPath; }
        public void SetPath(string path) { mPath = path; }
    }
}
