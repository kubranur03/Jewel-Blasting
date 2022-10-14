using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EslesmeBehaviour : MonoBehaviour
{
    Board board;

    public List<Mucevher> BulunanMucevherlerListe = new List<Mucevher>();

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void EslesmeleriBul()
    {
        BulunanMucevherlerListe.Clear();

        for (int x = 0; x < board.genislik; x++)
        {
            for (int y = 0; y < board.yukseklik; y++)
            {
                Mucevher gecerliMucevher = board.tumMucevherler[x, y];

                if (gecerliMucevher != null)
                {
                    //x sýrasýndaki eþleþmeler kontrol edildi
                    if (x > 0 && x < board.genislik - 1)
                    {
                        Mucevher solMucevher = board.tumMucevherler[x - 1, y];
                        Mucevher sagMucevher = board.tumMucevherler[x + 1, y];

                        if (solMucevher != null && sagMucevher != null)
                        {
                            if (solMucevher.tipi == gecerliMucevher.tipi && sagMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                solMucevher.eslestimi = true;
                                sagMucevher.eslestimi = true;
                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(solMucevher);
                                BulunanMucevherlerListe.Add(sagMucevher);
                            }
                        }

                    }
                    // y sýrasýndaki eþleþmeler kontrol edildi

                    if (y > 0 && y < board.yukseklik - 1)
                    {
                        Mucevher altMucevher = board.tumMucevherler[x, y - 1];
                        Mucevher ustMucevher = board.tumMucevherler[x, y + 1];

                        if (altMucevher != null && ustMucevher != null)
                        {
                            if (altMucevher.tipi == gecerliMucevher.tipi && ustMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                altMucevher.eslestimi = true;
                                ustMucevher.eslestimi = true;

                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(altMucevher);
                                BulunanMucevherlerListe.Add(ustMucevher);
                            }
                        }

                    }
                }
            }
        }
        //döngüler bitti
        if (BulunanMucevherlerListe.Count > 0)
        {
            BulunanMucevherlerListe = BulunanMucevherlerListe.Distinct().ToList();
        }
    }
}
