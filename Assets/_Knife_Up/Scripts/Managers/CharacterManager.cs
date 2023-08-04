﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OnefallGames
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Character Manager Configuration")]
        [SerializeField] private float characterSpace = 5f;
        [SerializeField] private float characterScale = 1.2f;
        [SerializeField] private float snapingCharacterTime = 0.3f;
        [SerializeField] private float swipeThresholdX = 5f;
        [SerializeField] [Range(0.1f, 1f)] private float scrollSpeedFactor = 0.25f;


        private List<CharacterInforController> listCharacterInforController = new List<CharacterInforController>();
        private CharacterInforController currentCharacterController = null;
        private Vector2 firstTouchPos = Vector2.zero;
        private float firstTouchTime = 0;
        private int currentCharacterIndex = 0;
        private bool isSnaping = false;

        private void Start()
        {
            //Load the view
            ViewManager.Instance.OnShowView(ViewType.CHARACTER_VIEW);

            //Create the characters
            currentCharacterIndex = Mathf.Clamp(ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex, 0, ServicesManager.Instance.CharacterContainer.CharacterInforControllers.Length - 1);
            for (int i = 0; i < ServicesManager.Instance.CharacterContainer.CharacterInforControllers.Length; i++)
            {
                int skinIndex = i - currentCharacterIndex;

                //Instantiate characters
                GameObject skinPrefab = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[i].gameObject;
                GameObject skin = Instantiate(skinPrefab, Vector3.zero, Quaternion.identity);
                CharacterInforController characterInfor = skin.GetComponent<CharacterInforController>();

                //Setup character
                characterInfor.SetSequenceNumber(i);
                skin.transform.localScale = Vector3.one;
                skin.transform.position = new Vector3(skinIndex * characterSpace, 0, 0);
                listCharacterInforController.Add(characterInfor);
            }

            //Rotate and scale the current CharacterInforController object
            currentCharacterController = listCharacterInforController[currentCharacterIndex];
            currentCharacterController.transform.localScale = Vector3.one * characterScale;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isSnaping)
            {
                firstTouchPos = Input.mousePosition;
                firstTouchTime = Time.time;
            }
            else if (Input.GetMouseButton(0) && !isSnaping)
            {
                Vector2 currentTouchPos = Input.mousePosition;
                float currentTouchTime = Time.time;
                float deltaX = Mathf.Abs(firstTouchPos.x - currentTouchPos.x);
                if (deltaX > swipeThresholdX)
                {
                    float speed = deltaX / (currentTouchTime - firstTouchTime);
                    Vector3 dir = (firstTouchPos.x - currentTouchPos.x < 0) ? Vector3.right : Vector3.left;
                    Vector3 moveVector = dir * (speed / 10) * scrollSpeedFactor * Time.deltaTime;

                    //Move all the character
                    for (int i = 0; i < listCharacterInforController.Count; i++)
                    {
                        // Move this character
                        listCharacterInforController[i].MoveByVector(moveVector);

                        //Caculate the scale this character
                        float d = Mathf.Abs(listCharacterInforController[i].transform.position.x);
                        if (d < (characterSpace / 2))
                        {
                            float factor = 1 - d / (characterSpace / 2);
                            Vector3 localScale = Mathf.Lerp(1, characterScale, factor) * Vector3.one;
                            listCharacterInforController[i].transform.localScale = localScale; ;
                        }
                        else
                        {
                            listCharacterInforController[i].transform.localScale = Vector3.one; ;
                        }
                    }

                    // Update for next step
                    firstTouchPos = currentTouchPos;
                    firstTouchTime = currentTouchTime;
                }
            }

            if (Input.GetMouseButtonUp(0) && !isSnaping)
            {
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.tick);

                //Find the skin nearest to center
                isSnaping = true;
                float snapDistance = 1000;
                CharacterInforController closestSkinControl = null;
                foreach (CharacterInforController o in listCharacterInforController)
                {
                    float disZ = Mathf.Abs(o.transform.position.x);
                    if (disZ < snapDistance)
                    {
                        closestSkinControl = o;
                        snapDistance = -o.transform.position.x;
                    }
                }

                //Snap all the characters, rotate the current character
                if (currentCharacterController != closestSkinControl)
                {
                    currentCharacterController = closestSkinControl;
                    ViewManager.Instance.CharacterViewController.UpdateUI(currentCharacterController);
                }
                StartCoroutine(CRSnaping(snapDistance));
            }

        }



        /// <summary>
        /// Snap current character.
        /// </summary>
        /// <param name="snapDistance"></param>
        /// <returns></returns>
        private IEnumerator CRSnaping(float snapDistance)
        {
            float snapDistanceAbs = Mathf.Abs(snapDistance);
            float snapSpeed = snapDistanceAbs / snapingCharacterTime;
            float sign = snapDistance / snapDistanceAbs;
            float movedDistance = 0;

            while (Mathf.Abs(movedDistance) < snapDistanceAbs)
            {
                float d = sign * snapSpeed * Time.deltaTime;
                float remainedDistance = Mathf.Abs(snapDistanceAbs - Mathf.Abs(movedDistance));
                d = Mathf.Clamp(d, -remainedDistance, remainedDistance);

                Vector3 moveVector = new Vector3(d, 0, 0);
                for (int i = 0; i < listCharacterInforController.Count; i++)
                {
                    // Move
                    listCharacterInforController[i].MoveByVector(moveVector);

                    // Scale and move forward according to distance from current position to center position
                    float dis = Mathf.Abs(listCharacterInforController[i].transform.position.x);
                    if (dis < (characterSpace / 2))
                    {
                        float factor = 1 - dis / (characterSpace / 2);
                        Vector3 localScale = Mathf.Lerp(1, characterScale, factor) * Vector3.one;
                        listCharacterInforController[i].transform.localScale = localScale;
                    }
                    else
                    {
                        listCharacterInforController[i].transform.localScale = Vector3.one;
                    }
                }

                movedDistance += d;
                yield return null;
            }

            isSnaping = false;
        }
    }
}
