// A slightly modified version of generic anti dumper. But this one works.

using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceProcess;
using System.Security.Principal;

namespace dotNETProtections
{
		
	public static class AntiDumperModule {
			
	[System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr ZeroMemory(IntPtr addr, IntPtr size);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr VirtualProtect(IntPtr lpAddress, IntPtr dwSize, IntPtr flNewProtect, ref IntPtr lpflOldProtect);

        private static void EraseSection(IntPtr address, int size)
        {
            IntPtr sz = (IntPtr)size;
            IntPtr dwOld = default(IntPtr);
            VirtualProtect(address, sz, (IntPtr)0x40, ref dwOld);
            ZeroMemory(address, sz);
            IntPtr temp = default(IntPtr);
            VirtualProtect(address, sz, dwOld, ref temp);
        }
        
        public static void Initialize()
        {
            var process             = System.Diagnostics.Process.GetCurrentProcess();
            var base_address        = process.MainModule.BaseAddress;
            var dwpeheader          = System.Runtime.InteropServices.Marshal.ReadInt32((IntPtr)(base_address.ToInt32() + 0x3C));
            var wnumberofsections   = System.Runtime.InteropServices.Marshal.ReadInt16((IntPtr)(base_address.ToInt32() + dwpeheader + 0x6));

            for (int i = 0; i < peheaderwords.Length; i++)
            {
               EraseSection((IntPtr)(base_address.ToInt32() + dwpeheader + peheaderwords[i]), 2);
            }
            for (int i = 0; i < peheaderbytes.Length; i++)
            {
               EraseSection((IntPtr)(base_address.ToInt32() + dwpeheader + peheaderbytes[i]), 1);
            }
            
            int x = 0;
            int y = 0;
           
            while (x <= wnumberofsections)
            {
                if (y == 0)
                {
                   EraseSection((IntPtr)((base_address.ToInt32() + dwpeheader + 0xFA + (0x28 * x)) + 0x20), 2);
                }

                
                y++;

                if (y == sectiontabledwords.Length)
                {
                    x++;
                    y = 0;
                }
            }
        }

        private static int[] sectiontabledwords = new int[] { 0x8, 0xC, 0x10, 0x14, 0x18, 0x1C, 0x24 };
        private static int[] peheaderbytes      = new int[] { 0x1A, 0x1B };
        private static int[] peheaderwords      = new int[] { 0x4, 0x16, 0x18, 0x40, 0x42, 0x44, 0x46, 0x48, 0x4A, 0x4C, 0x5C, 0x5E };
        private static int[] peheaderdwords     = new int[] { 0x0, 0x8, 0xC, 0x10, 0x16, 0x1C, 0x20, 0x28, 0x2C, 0x34, 0x3C, 0x4C, 0x50, 0x54, 0x58, 0x60, 0x64, 0x68, 0x6C, 0x70, 0x74, 0x104, 0x108, 0x10C, 0x110, 0x114, 0x11C };

			
		}
}
