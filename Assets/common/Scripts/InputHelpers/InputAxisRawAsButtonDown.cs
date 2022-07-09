using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.InputHelpers
{
    public class InputAxisRawAsButtonDown
    {
        private string axisName;
        private bool buttonBeingHeld;

        public InputAxisRawAsButtonDown(string axisName)
        {
            SetAxis(axisName);
            buttonBeingHeld = false;
        }

        public void SetAxis(string axisName)
        {
            this.axisName = axisName;
        }



        public float? GetAxisRawDown()
        {
            float input = Input.GetAxisRaw(this.axisName);

            if (IsButtonBeingHeld() && !buttonBeingHeld)
            {
                buttonBeingHeld = true;
                return input;
            }

            if (!IsButtonBeingHeld() && buttonBeingHeld)
            {
                buttonBeingHeld = false;
            }

            return null;
        }

        public float? GetAxisRawUp()
        {
            float input = Input.GetAxisRaw(this.axisName);

            if (IsButtonBeingHeld() && !buttonBeingHeld)
            {
                buttonBeingHeld = true;
            }

            if (!IsButtonBeingHeld() && buttonBeingHeld)
            {
                buttonBeingHeld = false;
                return input;
            }

            return null;
        }

        public float GetAxisRaw()
        {
            return Input.GetAxisRaw(this.axisName);
        }

        private bool IsButtonBeingHeld()
        {
            float input = Input.GetAxisRaw(this.axisName);
            return (input != 0);
        }
    }
}
