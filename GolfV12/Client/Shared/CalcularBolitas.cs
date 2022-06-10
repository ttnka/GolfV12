using GolfV12.Shared;

namespace GolfV12.Client.Shared
{
    public class CalcularBolitas
    {
        public List<TarjetaMolde> CalculoGeneral(IEnumerable<TarjetaMolde> LosScores, string TarjetaId, string ElPadreId,
            int[] Dif )
        {
            List<TarjetaMolde> BList = new List<TarjetaMolde>();
            if (LosScores != null)
            {
                foreach (var padre in LosScores)
                {
                    if (padre.UserId == ElPadreId)
                    {
                        int[] ElPadreScore = new int[18];
                        int[] PadreScore = new int[] {padre.H1,padre.H2,padre.H3,padre.H4,padre.H5,padre.H6,padre.H7,
                                            padre.H8,padre.H9,padre.H10,padre.H11,padre.H12,padre.H13,padre.H14,padre.H15,
                                            padre.H16,padre.H17,padre.H18 };
                        foreach (var hijo in LosScores)
                        {
                            if ( hijo.UserId != padre.UserId )
                            {
                                TarjetaMolde ElPadre = new TarjetaMolde();
                                ElPadre.UserId = padre.UserId;
                                ElPadre.Hijo = hijo.UserId;
                                ElPadre.Tarjeta = TarjetaId;
                                int[] HijoScore = new int[18] {hijo.H1,hijo.H2,hijo.H3,hijo.H4,hijo.H5,hijo.H6,hijo.H7,
                                            hijo.H8,hijo.H9,hijo.H10,hijo.H11,hijo.H12,hijo.H13,hijo.H14,hijo.H15,
                                            hijo.H16,hijo.H17,hijo.H18};
                                for (int h = 0; h < 18; h++)
                                {
                                    if (PadreScore[h] > 0 && HijoScore[h] > 0)
                                    {
                                        ElPadreScore[h] = CalcularB(padre.Hcp, hijo.Hcp, Dif[h],
                                            PadreScore[h], HijoScore[h]);
                                    }
                                }
                                {
                                    ElPadre.H1 = ElPadreScore[0];
                                    ElPadre.H2 = ElPadreScore[1];
                                    ElPadre.H3 = ElPadreScore[2];
                                    ElPadre.H4 = ElPadreScore[3];
                                    ElPadre.H5 = ElPadreScore[4];
                                    ElPadre.H6 = ElPadreScore[5];
                                    ElPadre.H7 = ElPadreScore[6];
                                    ElPadre.H8 = ElPadreScore[7];
                                    ElPadre.H9 = ElPadreScore[8];
                                    ElPadre.H10 = ElPadreScore[9];
                                    ElPadre.H11 = ElPadreScore[10];
                                    ElPadre.H12 = ElPadreScore[11];
                                    ElPadre.H13 = ElPadreScore[12];
                                    ElPadre.H14 = ElPadreScore[13];
                                    ElPadre.H15 = ElPadreScore[14];
                                    ElPadre.H16 = ElPadreScore[15];
                                    ElPadre.H17 = ElPadreScore[16];
                                    ElPadre.H18 = ElPadreScore[17];
                                }
                                BList.Add(ElPadre);
                            }
                        }
                    }
                }
            }
            return BList;
        }
        public int CalcularB(int PHcp, int HHcp, int HoyoH, int PadreScore, int HijoScore )
                {
                    int signo    = PHcp < HHcp ? -1 : 1;
                    int bolita   = Math.Abs(PHcp - HHcp) / 18;
                    bolita      += Math.Abs(PHcp - HHcp) % 18 >= HoyoH ? 1 : 0;            
                    bolita      = PadreScore - bolita * signo - HijoScore;

                    if ( bolita == 0) return 0;
                    return bolita < 0 ? 1 : -1;
                }

/*
        protected void Calcular()
        {
            List<TarjetaMolde> BList = new List<TarjetaMolde>();
            if (LosScores != null)
            {
                foreach (var padre in LosScores)
                {
                    if (padre.UserId == ElPadreId)
                    {
                        int[] ElPadreScore = new int[18];
                        int[] PadreScore = new int[] {padre.H1,padre.H2,padre.H3,padre.H4,padre.H5,padre.H6,padre.H7,
                                            padre.H8,padre.H9,padre.H10,padre.H11,padre.H12,padre.H13,padre.H14,padre.H15,
                                            padre.H16,padre.H17,padre.H18 };

                        foreach (var hijo in LosScores)
                        {
                            if (padre.UserId != hijo.UserId)
                            {
                                TarjetaMolde ElPadre = new TarjetaMolde();
                                ElPadre.UserId = padre.UserId;
                                ElPadre.Hijo = hijo.UserId;
                                ElPadre.Tarjeta = TarjetaId;

                                int[] HijoScore = new int[18] {hijo.H1,hijo.H2,hijo.H3,hijo.H4,hijo.H5,hijo.H6,hijo.H7,
                                            hijo.H8,hijo.H9,hijo.H10,hijo.H11,hijo.H12,hijo.H13,hijo.H14,hijo.H15,
                                            hijo.H16,hijo.H17,hijo.H18};

                                for (int h = 0; h < 18; h++)
                                {
                                    if (PadreScore[h] > 0 && HijoScore[h] > 0)
                                    {
                                        ElPadreScore[h] = ElCalculo.CalcularB(padre.Hcp, hijo.Hcp, int.Parse(DatosDic[$"HoyoH_{h + 1}"]),
                                            PadreScore[h], HijoScore[h]);
                                    }
                                }
                                // Alimentar ElPadre
                                {
                                    ElPadre.H1 = ElPadreScore[0];
                                    ElPadre.H2 = ElPadreScore[1];
                                    ElPadre.H3 = ElPadreScore[2];
                                    ElPadre.H4 = ElPadreScore[3];
                                    ElPadre.H5 = ElPadreScore[4];
                                    ElPadre.H6 = ElPadreScore[5];
                                    ElPadre.H7 = ElPadreScore[6];
                                    ElPadre.H8 = ElPadreScore[7];
                                    ElPadre.H9 = ElPadreScore[8];
                                    ElPadre.H10 = ElPadreScore[9];
                                    ElPadre.H11 = ElPadreScore[10];
                                    ElPadre.H12 = ElPadreScore[11];
                                    ElPadre.H13 = ElPadreScore[12];
                                    ElPadre.H14 = ElPadreScore[13];
                                    ElPadre.H15 = ElPadreScore[14];
                                    ElPadre.H16 = ElPadreScore[15];
                                    ElPadre.H17 = ElPadreScore[16];
                                    ElPadre.H18 = ElPadreScore[17];
                                }
                                BList.Add(ElPadre);
                                // Alimenta BolitasDic 
                                if (!LasBolitasDic.ContainsKey(padre.UserId))
                                {
                                    G320Bolitas g32b = new G320Bolitas();
                                    g32b.Tarjeta = ElPadre.Tarjeta;
                                    g32b.Azar = "Bolitas";
                                    g32b.J1 = ElPadre.UserId;
                                    g32b.J2 = ElPadre.Hijo;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H2V = ElPadre.H1;
                                    g32b.H3V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;
                                    g32b.H1V = ElPadre.H1;

                                    LasBolitasDic.Add(padre.UserId, g32b);
                                }
                            }
                        }
                    }
                }
            }
            LasBolitas = BList.AsEnumerable();
        }
*/
    }

}
