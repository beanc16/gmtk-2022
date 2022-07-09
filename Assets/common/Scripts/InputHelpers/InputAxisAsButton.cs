using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.InputHelpers
{
    public class InputAxisAsButton
    {
        private string axisName;
        private bool buttonBeingHeld;
        private float previousUpInput;

        public InputAxisAsButton(string axisName)
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
            float temp = previousUpInput;
            previousUpInput = input;

            if (IsButtonBeingHeld() && !buttonBeingHeld)
            {
                buttonBeingHeld = true;
            }

            if (!IsButtonBeingHeld() && buttonBeingHeld)
            {
                buttonBeingHeld = false;
                return temp;
            }

            return null;
        }

        public float GetAxisRaw()
        {
            return Input.GetAxisRaw(this.axisName);
        }



        public float? GetAxisDown()
        {
            float input = Input.GetAxis(this.axisName);

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

        public float? GetAxisUp()
        {
            float input = Input.GetAxis(this.axisName);
            float temp = previousUpInput;
            previousUpInput = input;

            if (IsButtonBeingHeld() && !buttonBeingHeld)
            {
                buttonBeingHeld = true;
            }

            if (!IsButtonBeingHeld() && buttonBeingHeld)
            {
                buttonBeingHeld = false;
                return temp;
            }

            return null;
        }

        public float GetAxis()
        {
            return Input.GetAxis(this.axisName);
        }



        private bool IsButtonBeingHeld()
        {
            float input = Input.GetAxisRaw(this.axisName);
            return (input != 0);
        }
    }
}
