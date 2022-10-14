using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public int genislik;
    public int yukseklik;

    public GameObject tilePrefab;

    public Mucevher[] mucevherler;

    public Mucevher[,] tumMucevherler;

    public float MucevherHizi;

    public EslesmeBehaviour eslesmeBehaviour;

    public enum BoardDurum { bekliyor, hareketEdiyor };
    public BoardDurum gecerliDurum = BoardDurum.hareketEdiyor;



    private void Awake()
    {
        eslesmeBehaviour = Object.FindObjectOfType<EslesmeBehaviour>();
    }



    private void Start()
    {
        tumMucevherler = new Mucevher[genislik, yukseklik];

        Duzenle();
    }




    void Duzenle()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < genislik; y++)
            {
                Vector2 pos = new Vector2(x, y);

                GameObject bgTile = Instantiate(tilePrefab, pos, Quaternion.identity);


                bgTile.transform.parent = this.transform;
                bgTile.name = "BG Tile -" + x + ", " + y;

                int RastgeleMucevher = Random.Range(0, mucevherler.Length);

                int kontrolSayaci = 0;
                while (EslesmeVarmi(new Vector2Int(x, y), mucevherler[RastgeleMucevher]) && kontrolSayaci < 100)
                {
                    RastgeleMucevher = Random.Range(0, mucevherler.Length);

                    kontrolSayaci++;
                }



                MucevherOlustur(new Vector2Int(x, y), mucevherler[RastgeleMucevher]);


                MucevherOlustur(new Vector2Int(x, y), mucevherler[RastgeleMucevher]);
            }
        }
    }



    void MucevherOlustur(Vector2Int pos, Mucevher olusacakMucevher)
    {
        Mucevher mucevher = Instantiate(olusacakMucevher, new Vector3(pos.x, pos.y+yukseklik, 0f), Quaternion.identity);
        mucevher.transform.parent = this.transform;
        mucevher.name = "Mucevher -" + pos.x + " " + pos.y;


        tumMucevherler[pos.x, pos.y] = mucevher;


        mucevher.MucevherDuzenle(pos, this);
    }


    bool EslesmeVarmi(Vector2Int posKontrol, Mucevher kontrolEdilenMucevher)
    {
        if (posKontrol.x > 1)
        {
            if (tumMucevherler[posKontrol.x - 1, posKontrol.y].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x - 2, posKontrol.y].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }

        }

        if (posKontrol.y > 1)
        {
            if (tumMucevherler[posKontrol.x, posKontrol.y - 1].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x, posKontrol.y - 2].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }

        }
        return false;

    }


    void EslesenMucevheriYokEt(Vector2Int pos)
    {
        if (tumMucevherler[pos.x, pos.y] != null)
        {
            if (tumMucevherler[pos.x, pos.y].eslestimi)
            {
                Destroy(tumMucevherler[pos.x, pos.y].gameObject);
                tumMucevherler[pos.x, pos.y] = null;
            }
        }
    }


    public void TumEslesenleriYokEt()
    {
        for (int i = 0; i < eslesmeBehaviour.BulunanMucevherlerListe.Count; i++)
        {
            if (eslesmeBehaviour.BulunanMucevherlerListe[i] != null)
            {
                EslesenMucevheriYokEt(eslesmeBehaviour.BulunanMucevherlerListe[i].posIndex);
            }
        }
        StartCoroutine(AltaKaydirRoutine());
    }


    IEnumerator AltaKaydirRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        int boslukSayac = 0;

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (tumMucevherler[x, y] == null)
                {
                    boslukSayac++;
                }
                else if (boslukSayac > 0)
                {
                    tumMucevherler[x, y].posIndex.y -= boslukSayac;
                    tumMucevherler[x, y - boslukSayac] = tumMucevherler[x, y];
                    tumMucevherler[x, y] = null;
                }
            }

            boslukSayac = 0;

        }

        StartCoroutine(BoardYenidenDoldurRouitine());
    }


    IEnumerator BoardYenidenDoldurRouitine()
    {
        yield return new WaitForSeconds(.5f);
        UstBosluklariDoldur();

        yield return new WaitForSeconds(0.5f);

        eslesmeBehaviour.EslesmeleriBul();

        if (eslesmeBehaviour.BulunanMucevherlerListe.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
            TumEslesenleriYokEt();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            gecerliDurum = BoardDurum.hareketEdiyor;

        }
    }

    void UstBosluklariDoldur()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (tumMucevherler[x, y] == null)
                {
                    int RastgeleMucevher = Random.Range(0, mucevherler.Length);
                    MucevherOlustur(new Vector2Int(x, y), mucevherler[RastgeleMucevher]);
                }

            }
        }

        YanlisYerlertirmeleriKontrolEt();
    }

    void YanlisYerlertirmeleriKontrolEt()
    {
        List<Mucevher> bulunanMucevherList = new List<Mucevher>();

        bulunanMucevherList.AddRange(FindObjectsOfType<Mucevher>());
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (bulunanMucevherList.Contains(tumMucevherler[x, y]))
                {
                    bulunanMucevherList.Remove(tumMucevherler[x, y]);
                }



            }
        }

        foreach (Mucevher mucevher in bulunanMucevherList)
        {
            Destroy(mucevher.gameObject);
        }
    }

    public void BoardKaristir()
    {
        if (gecerliDurum != BoardDurum.bekliyor)
        {
            gecerliDurum = BoardDurum.bekliyor;

            List<Mucevher> sahnedekiMucevherlerList = new List<Mucevher>();

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    sahnedekiMucevherlerList.Add(tumMucevherler[x, y]);
                    tumMucevherler[x, y] = null;

                }
            }

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    int kullanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);

                    int kontrolSayac = 0;

                    while (EslesmeVarmi(new Vector2Int(x, y), sahnedekiMucevherlerList[kullanilacakMucevher]) && kontrolSayac < 100 && sahnedekiMucevherlerList.Count > 1)
                    {

                        kullanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);
                        kontrolSayac++;
                    }


                    sahnedekiMucevherlerList[kullanilacakMucevher].MucevherDuzenle(new Vector2Int(x, y), this);
                    tumMucevherler[x, y] = sahnedekiMucevherlerList[kullanilacakMucevher];
                    sahnedekiMucevherlerList.RemoveAt(kullanilacakMucevher);
                }
            }
            StartCoroutine(AltaKaydirRoutine());
        }
    }
}

