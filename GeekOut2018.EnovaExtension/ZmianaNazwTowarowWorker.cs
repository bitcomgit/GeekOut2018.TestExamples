﻿using System.Linq;
using GeekOut2018.EnovaExtension;
using Soneta.Business;
using Soneta.Tools;
using Soneta.Towary;
using Soneta.Types;

[assembly: Worker(typeof(ZmianaNazwTowarowWorker), typeof(Towary))]

namespace GeekOut2018.EnovaExtension
{
    /// <summary>
    /// This worker is based on Example 5 from soneta.Examples - https://github.com/soneta/Examples
    /// </summary>
    public class ZmianaNazwTowarowWorker : ContextBase
    {
        public ZmianaNazwTowarowWorker(Context context) : base(context)
        {
        }

        // Potrzebne dla akcji parametry
        [Context]
        public ZmianaNazwTowarowParams Params
        {
            get;
            set;
        }

        // Potrzebne dane na których zostanie wykonana akcja
        [Context]
        public Towar[] Towary
        {
            get; set;
        }

        // Akcja jaka zostanie wykonana na danych w oparciu o ustawione parametry
        [Action("Soneta Examples/Zmiana postfiksu", Mode = ActionMode.SingleSession | ActionMode.ConfirmSave | ActionMode.Progress)]
        public void ZmianaNazw()
        {
            using (var t = Params.Session.Logout(true))
            {
                foreach (var towar in Towary.Where(towar => Params.TypTowaru == towar.Typ))
                {
                    if (!Params.DodajPostfiks.IsNullOrEmpty() && !towar.Nazwa.StartsWith(Params.DodajPostfiks))
                    {
                        towar.Nazwa = towar.Nazwa + Params.DodajPostfiks;
                    }

                    if (!Params.UsunPostfiks.IsNullOrEmpty() && towar.Nazwa.EndsWith(Params.UsunPostfiks))
                    {
                        towar.Nazwa = towar.Nazwa.Substring(0, towar.Nazwa.Length - Params.UsunPostfiks.Length);
                    }
                }
                t.Commit();
            }
        }
    }

    public class ZmianaNazwTowarowParams : ContextBase
    {
        public ZmianaNazwTowarowParams(Context context) : base(context)
        {
            TypTowaru = TypTowaru.Towar;
        }

        public TypTowaru TypTowaru { get; set; }

        public string DodajPostfiks { get; set; }

        [Caption("Usuń postfiks")]
        public string UsunPostfiks { get; set; }
    }
}
