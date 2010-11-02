using System;
using System.Collections.Generic;
using System.Text;

namespace LLRP
{
    public class Tag
    {
        private string epc;
        private int count;

        /// <summary>
        /// Constructs a Tag for the given epc
        /// </summary>
        /// <param name="epc"></param>
        public Tag(string epc)
        {
            this.epc = epc;
            this.count = 1;
        }

        /// <summary>
        /// Increment the count for this tag
        /// </summary>
        public void IncrementCount()
        {
            count++;
        }
        /// <summary>
        /// Gets the current count for this tag
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return this.count;
        }

        /// <summary>
        /// Returns the epc identifier of this tag
        /// </summary>
        /// <returns></returns>

        public string GetEPC()
        {
            return this.epc;
        }

        /// <summary>
        /// Return true if the current epc id equals the passed epc id 
        /// </summary>
        /// <param name="otherEpc">
        /// The epc to be compare
        /// </param>
        /// <returns></returns>
        public bool CompareEPC(string otherEpc)
        {
            return this.epc.Equals(otherEpc);

        }

        /// <summary>
        /// Returns a string representation of the tag and its count
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("EPC = {0},    Count = {1}", this.epc, this.count);
        }

    }
}
