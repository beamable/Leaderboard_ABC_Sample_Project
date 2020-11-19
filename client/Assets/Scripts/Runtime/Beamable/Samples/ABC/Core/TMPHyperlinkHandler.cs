﻿using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Beamable.Samples.ABC.Core
{
   /// <summary>
   /// Open a web browser for any <link=blah>text</link> in the source text.
   /// <see cref="https://deltadreamgames.com/unity-tmp-hyperlinks/"/>
   /// </summary>
   public class TMPHyperlinkHandler : MonoBehaviour, IPointerClickHandler
   {
      //  Fields ---------------------------------------
      [SerializeField]
      private TextMeshProUGUI _textMesh = null;

      //  Unity Methods -------------------------------
      protected void Start()
      {
         Assert.IsNotNull(_textMesh);
      }

      //  Event Handlers -------------------------------
      public void OnPointerClick(PointerEventData eventData)
      {
         int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMesh, Input.mousePosition, Camera.main);
         if (linkIndex != -1)
         {
            TMP_LinkInfo linkInfo = _textMesh.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
         }
      }
   }
}
