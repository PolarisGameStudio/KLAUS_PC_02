using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateLabel : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        if (SaveManager.Instance.dataKlaus.language.Contains("en"))
        {
            if (text.text == "Move Horizontally")
            {
                text.text = "Move Horizontally";
            }

            else if (text.text == "Move Right")
            {

            }

            else if (text.text == "Move Left")
            {

            }

            else if (text.text == "Move Vertically")
            {

            }

            else if (text.text == "Move UP")
            {
                text.text = "Move Up";
            }

            else if (text.text == "Move Down")
            {

            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {

            }

            else if (text.text == "Run")
            {

            }

            else if (text.text == "Hack / Punch")
            {

            }

            else if (text.text == "Throw")
            {
                text.text = "Throw (K1)";
            }

            else if (text.text == "Switch Character")
            {

            }

            else if (text.text == "Control Both Characters")
            {

            }

            else if (text.text == "Target")
            {

            }

            else if (text.text == "Enable Move Camera")
            {

            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Camera Horizontally";
            }
            else if (text.text == "Movement Right Camera")
            {

            }

            else if (text.text == "Movement Left Camera")
            {

            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Camera Vertical";
            }

            else if (text.text == "Movement Up Camera")
            {

            }

            else if (text.text == "Movement Down Camera")
            {

            }


        }

        else if (SaveManager.Instance.dataKlaus.language.Contains("de"))
        {

            if (text.text == "Move Horizontally")
            {
                text.text = "Horizontal";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Rechts";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Links";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Vertikale";
            }

            else if (text.text == "Move UP")
            {
                text.text = "Oben";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Nieder";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Springen / [Halten] Gleiten (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Laufen";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Hacken / Schlagen";
            }

            else if (text.text == "Throw")
            {
                text.text = "Werfe (K1)";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Figur wechseln"; 
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Steuert beide Charaktere";
            }

            else if (text.text == "Target")
            {
                text.text = "Ziel";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Kamera bewegen";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Kamera horizontal";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Kamera rechts";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Kamera links";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Kamera vertikal";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Kamera hoch";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Kamera runter";
            }


        }

        else if (SaveManager.Instance.dataKlaus.language.Contains("it"))
        {

            if (text.text == "Move Horizontally")
            {
                text.text = "Sposta orizzontalmente";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Destra";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Sinistra";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Sposta in verticale";
            }

            else if (text.text == "Move UP")
            {
                text.text = "Su";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Giù";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Salta / [Tenere] Plana (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Corri";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Hackera/Tira un pugno";
            }

            else if (text.text == "Throw")
            {
                text.text = "Tira (K1)";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Cambia personaggio";
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Controllare entrambi i personaggi";
            }

            else if (text.text == "Target")
            {
                text.text = "Bersaglio";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Muovi telecamera";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Telecamera orizzontale";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Telecamera a destra";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Telecamera sinistra";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Telecamera verticale";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Telecamera su";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Telecamera giù";
            }
        }

        else if (SaveManager.Instance.dataKlaus.language.Contains("fr"))
        {
            if (text.text == "Move Horizontally")
            {
                text.text = "Déplacer horizontalement";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Droite";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Gauche";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Déplacer verticalement";
            }

            else if (text.text == "Move UP")
            {
                text.text = "En haut";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Vers le bas";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Sauter / [Tenir] Planer (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Courir";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Pirater / Coup de poing";
            }

            else if (text.text == "Throw")
            {
                text.text = "Lancer (K1)";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Changer personnage";
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Contrôler les deux personnages";
            }

            else if (text.text == "Target")
            {
                text.text = "Cible";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Bouger la caméra";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Caméra horizontale";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Déplacer la caméra vers la droite";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Déplacer la caméra vers la gauche";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Caméra verticale";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Déplacer la caméra vers le haut";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Déplacer la caméra vers le bas";
            }
        }

        else if (SaveManager.Instance.dataKlaus.language.Contains("es"))
        {
            if (text.text == "Move Horizontally")
            {
                text.text = "Mover horizontalmente";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Derecha";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Izquierda";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Mover Verticalmente";
            }

            else if (text.text == "Move UP")
            {
                text.text = "Arriba";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Abajo";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Saltar / [Sostenido] Planear (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Correr";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Hackear / Golpear";
            }

            else if (text.text == "Throw")
            {
                text.text = "Lanzar (K1)";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Cambiar de personaje";
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Controlar ambos personajes";
            }

            else if (text.text == "Target")
            {
                text.text = "Objetivo";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Mover Camara";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Mover Camara Horizontalmente";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Camara Derecha";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Camara Izquierda";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Camara Vertical";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Camara Arriba";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Camara Abajo";
            }
        }

        else if (SaveManager.Instance.dataKlaus.language.Contains("pt"))
        {
            if (text.text == "Move Horizontally")
            {
                text.text = "Mover horizontalmente";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Mova para a direita";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Mova à esquerda";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Mover verticalmente";
            }

            else if (text.text == "Move UP")
            {
                text.text = "Subir";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Descer";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Pular /  [Manter] Planar (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Correr";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Hackear / Socar";
            }

            else if (text.text == "Throw")
            {
                text.text = "Lançar (K1";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Mudar de personagem";
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Controlar ambos os personagens";
            }

            else if (text.text == "Target")
            {
                text.text = "Alvo";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Mover câmera";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Câmera horizontal";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Câmera certa";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Câmera esquerda";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Câmera Vertical";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Câmera para cima";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Câmera para baixo";
            }
        }
        else if (SaveManager.Instance.dataKlaus.language.Contains("ru"))
        {

            if (text.text == "Move Horizontally")
            {
                text.text = "Двигаться горизонтально";
            }

            else if (text.text == "Move Right")
            {
                text.text = "Двигаться вправо";
            }

            else if (text.text == "Move Left")
            {
                text.text = "Движение влево";
            }

            else if (text.text == "Move Vertically")
            {
                text.text = "Двигаться вертикально";
            }

            else if (text.text == "Move UP")
            {
                text.text = "Двигаться вверх";
            }

            else if (text.text == "Move Down")
            {
                text.text = "Двигаться вниз";
            }

            else if (text.text == "Jump / [Hold] Glide (K1)")
            {
                text.text = "Прыгать / [Держать] поплавок (K1)";
            }

            else if (text.text == "Run")
            {
                text.text = "Бежать";
            }

            else if (text.text == "Hack / Punch")
            {
                text.text = "Взломать / Удар";
            }

            else if (text.text == "Throw")
            {
                text.text = "Бросить (K1)";
            }

            else if (text.text == "Switch Character")
            {
                text.text = "Переключить персонажа";
            }

            else if (text.text == "Control Both Characters")
            {
                text.text = "Управлять обоими персонажами";
            }

            else if (text.text == "Target")
            {
                text.text = "Цель";
            }

            else if (text.text == "Enable Move Camera")
            {
                text.text = "Переместить камеру";
            }

            else if (text.text == "Movement Camera X")
            {
                text.text = "Движение камеры по горизонтали";
            }
            else if (text.text == "Movement Right Camera")
            {
                text.text = "Движение камеры вправо";
            }

            else if (text.text == "Movement Left Camera")
            {
                text.text = "Движение камеры слева";
            }

            else if (text.text == "Movement Camera Y")
            {
                text.text = "Переместить камеру вертикально";
            }

            else if (text.text == "Movement Up Camera")
            {
                text.text = "Переместить камеру вверх";

            }

            else if (text.text == "Movement Down Camera")
            {
                text.text = "Переместить камеру вниз";
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
