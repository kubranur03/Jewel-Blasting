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
    float sürüklemeAcisi;


    public void MucevherDuzenle(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void Update()
    {
        if(mouseBasildi && Input.GetMouseButtonUp(0))
        {
            mouseBasildi = false;

            sonBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            AciyiHesapla();
           
        }
    }

    private void OnMouseDown()
    {
        birinciBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseBasildi = true;
    }

    void AciyiHesapla()
    {
        float dx = sonBasilanPos.x - birinciBasilanPos.x;
        float dy = sonBasilanPos.y - birinciBasilanPos.y;

        sürüklemeAcisi = Mathf.Atan2(dy, dx);
        sürüklemeAcisi = sürüklemeAcisi * 180 / Mathf.PI;

        print(sürüklemeAcisi);


    }
}
