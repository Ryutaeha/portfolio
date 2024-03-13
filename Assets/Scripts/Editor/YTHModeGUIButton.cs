using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(YTHModeScript))]
public class YTHModeGUIButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 현재 선택된 오브젝트
        YTHModeScript tester = (YTHModeScript)target;

        // 버튼을 생성하고 클릭되면 호출될 함수를 설정
        //if (GUILayout.Button("버튼 이름"))
        //{
        //    Tester.cs에 함수 작성 후에 여기서 호출하면 됩니다.
        //    tester.함수명();
        //}
        //tester.scene = (Define.Scene)EditorGUILayout.EnumPopup("Scene", tester.scene);

        if (GUILayout.Button("테스트 씬으로"))
        {
            tester.TestYTHSceneLord();
        }
    }
}