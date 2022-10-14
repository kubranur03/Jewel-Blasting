using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mucevher : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;

    [HideInInspector]
    public Board board;


    public Vector2 birinciBasilanPos;
    public Vector2 sonBasilanPos;

    bool mouseBasildi;
    float s�r�klemeAcisi;

    Mucevher digerMucevher;

    public enum MucevherTipi { mavi, pembe, sari, acikYesil, koyuYesil, bomba };

    public MucevherTipi tipi;

    public bool eslestimi;

    Vector2Int ilkPos;




    public void MucevherDuzenle(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }



    private void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.MucevherHizi * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
        }

        if (mouseBasildi && Input.GetMouseButtonUp(0))
        {
            mouseBasildi = false;

            if (board.gecerliDurum == Board.BoardDurum.hareketEdiyor)
            {
                sonBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AciyiHesapla();
            }

        }
    }



    private void OnMouseDown()
    {
        if (board.gecerliDurum == Board.BoardDurum.hareketEdiyor)
        {
            birinciBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseBasildi = true;

        }
        birinciBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseBasildi = true;
    }


    void AciyiHesapla()
    {
        float dx = sonBasilanPos.x - birinciBasilanPos.x;
        float dy = sonBasilanPos.y - birinciBasilanPos.y;

        s�r�klemeAcisi = Mathf.Atan2(dy, dx);
        s�r�klemeAcisi = s�r�klemeAcisi * 180 / Mathf.PI;

        if (Vector3.Distance(birinciBasilanPos, sonBasilanPos) > 0.5f)
        {
            TileHareket();
        }
    }



        void TileHareket()
        {
            ilkPos = posIndex;

            if (s�r�klemeAcisi < 45 && s�r�klemeAcisi > -45 && posIndex.x < board.genislik - 1)
            {
                digerMucevher = board.tumMucevherler[posIndex.x + 1, posIndex.y];
                digerMucevher.posIndex.x--;
                posIndex.x++;
            }

            else if (s�r�klemeAcisi > 45 && s�r�klemeAcisi <= 135 && posIndex.y < board.yukseklik - 1)
            {
                digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y + 1];
                digerMucevher.posIndex.y--;
                posIndex.y++;
            }

            else if (s�r�klemeAcisi < -45 && s�r�klemeAcisi >= -135 && posIndex.y > 0)
            {
                digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y - 1];
                digerMucevher.posIndex.y++;
                posIndex.y--;
            }

            else if (s�r�klemeAcisi > 135 || s�r�klemeAcisi >= -135 && posIndex.x > 0)
            {
                digerMucevher = board.tumMucevherler[posIndex.x - 1, posIndex.y];
                digerMucevher.posIndex.x++;
                posIndex.x--;
            }

        board.tumMucevherler[posIndex.x, posIndex.y] = this;
        board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

        StartCoroutine(HareketiKontrolEtRouitne());
    }


    public IEnumerator HareketiKontrolEtRouitne()
    {
        board.gecerliDurum = Board.BoardDurum.bekliyor;

        yield return new WaitForSeconds(.5f);

        board.eslesmeBehaviour.EslesmeleriBul();

        if (digerMucevher != null)
        {
            if (!eslestimi && !digerMucevher.eslestimi)
            {
                digerMucevher.posIndex = posIndex;
                posIndex = ilkPos;

                board.tumMucevherler[posIndex.x, posIndex.y] = this;
                board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

                yield return new WaitForSeconds(.5f);
                board.gecerliDurum = Board.BoardDurum.hareketEdiyor;
            }
            else
            {
                board.TumEslesenleriYokEt();
            }
        }
    }

}

