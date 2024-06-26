using UnityEngine;

namespace TLab.InputField
{
    public class Key : KeyBase
    {
        [SerializeField] private string m_lower;
        [SerializeField] private string m_upper;

        public override void OnPress()
        {
            Debug.Log("Key OnPress: " + m_lower);
            keyborad.OnKeyPress(keyborad.shift ? m_upper : m_lower);
        }

        private void SwitchDisp()
        {
            if (keyborad != null)
            {
                if (keyborad.gameObject.activeSelf)
                {
                    m_upperDisp.SetActive(keyborad.shift);
                    m_lowerDisp.SetActive(!keyborad.shift);
                }
            }
        }

        public override void OnShift()
        {
            SwitchDisp();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            SwitchDisp();

            //Here starts my changes
            //BoxCollider col = gameObject.AddComponent<BoxCollider>();
            //col.isTrigger = true; // Disabled because working on climb 
            //col.providesContacts = true;
            ////col.size = new Vector3(70, 70, 5);
            //col.size = new Vector3(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height, 5);
            //Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
            //rigidbody.isKinematic = true;
            tag = "Key";
        }

        private void OnTriggerEnter(Collider other)
        {
            ///TODO: CORRECT THIS
            //keyborad.OnKeyPress(keyborad.shift ? m_upper : m_lower);
        }

#if UNITY_EDITOR
        public override void Setup()
        {
            base.Setup();

            string[] split = gameObject.name.Split(" ");
            switch (split.Length)
            {
                case 1:
                    m_lower = split[0];
                    m_upper = split[0];
                    break;
                case 2:
                    m_lower = split[0];
                    m_upper = split[1];
                    break;
            }

            m_lowerDisp = transform.GetChild(0).gameObject;
            m_upperDisp = transform.GetChild(1).gameObject;

            m_lowerDisp.SetActive(true);
            m_upperDisp.SetActive(false);
        }
#endif
    }
}
