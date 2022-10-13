using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int genislik;
    public int yukseklik;

    public GameObject tilePrefab;

    public Mucevher[] mucevherler;

    public Mucevher[,] tumMucevherler;

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

                MucevherOlustur(new Vector2Int(x, y), mucevherler[RastgeleMucevher]);
            }
        }
    }

    void MucevherOlustur(Vector2Int pos, Mucevher olusacakMucevher)
    {
        Mucevher mucevher = Instantiate(olusacakMucevher, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        mucevher.transform.parent = this.transform;
        mucevher.name = "Mucevher -" + pos.x + " "+pos.y;


        tumMucevherler[pos.x, pos.y] = mucevher;


        mucevher.MucevherDuzenle(pos, this);
    }

}
