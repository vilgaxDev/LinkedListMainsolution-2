/*
 * This is the main program to demonstrate linked list implementation
 * 
 */
using System;
using System.IO;
using System.Text;
using System.Collections;
using LinkedListLib;

namespace LinkedListMain
{	
	class MainClass
	{	
		// Globals
		const int HEADER = 1;
		const int GRP = 2;
		
		public static int iState = 0;
		public static string sInRec = null;
		public static string sType = null;
		
		public static LinkedList MyList = new LinkedList();
	
		public static LlNode MyNode = new LlNode();
		
		public static void Main()
		{	
			string sInfilename = null;  	// Storage for the input filename
			
			// String array args will contain pointers to each command line argument.
			String[ ] args = Environment.GetCommandLineArgs( );
			
			// If only the program name was entered on the command line then generate
			// an error because filename is required.
			if (args.Length == 1) {
				Console.WriteLine ("Filename is required.");
				Environment.Exit(1);
			}
			else
				sInfilename = args[1];
		
			StreamReader oInfile = File.OpenText(sInfilename);
			
			sInRec = oInfile.ReadLine();
		
			iState = HEADER;
			
			while (sInRec != null) {
				Process();
				sInRec = oInfile.ReadLine();
			}
			
			PrintMyList(0); // Print the grouping node
			PrintMyList(1); // Print all segments in the group (branch)
			
			oInfile.Close();
				
			Console.WriteLine("End of program.");
			
		} // *** Main ***		
		
		public static void Process() 
		{
			sType = sInRec.Substring(0,3);
			string sData = sInRec;

			bool bExists = false;
			int pindex = 0;
			LinkedList temp = new LinkedList();
			
			if (sType == "HDR") {
				iState = HEADER;
			}
			
			if (sType == "GRP" && iState == HEADER) {
				MyNode.status = "Initialized";
				iState = GRP; }
			else
				if (sType == "GRP" && iState == GRP) {
					MyNode.data = "MyNode";
					MyList.PushNode(MyNode, 0);
	
					Console.WriteLine("Show list");
					MyList.PrintList();
				
					bExists = MyList.Exists(MyNode,"GRP",ref pindex);
					PrintMyList(0); // Print the grouping node
					PrintMyList(1); // Print all segments in the group (branch)
				}
			
			switch (iState) {
				case HEADER:
					ProcessHdr();
					break;
				
				case GRP:
					MyList = new LinkedList();
					ProcessSegment();
					break;
					
			} // *** switch ***
			
		} // *** Process ***
		
		public static void PrintMyList(int pindex)
		{
			LinkedList temp = new LinkedList();
			temp = (LinkedList)MyNode.minornode[pindex];
			temp.PrintList();
			
		} // *** PrintList ***
		
		public static void ProcessHdr()
		{	
			Console.WriteLine(sType + " ProcessHdr");
		
		} // *** ProcessHdr ***
		
		public static void ProcessSegment()
		{
			LinkedList tempnode = new LinkedList();
			LinkedList templist = new LinkedList();
			bool bexists = false;
			int pindex = 0;
			
			switch (sType) {
				case "GRP":
					// We can put some checks here to make sure
					// that we do not add more than one GRP per grouping
					
					// This is the first segment in this loop and thus a 
					// new grouping List node is declared;
					MyNode = new LlNode();

					// node is the ListNode object where the segment's data
					// is stored.
					LlNode node = new LlNode();
					
					// templist will ultimately end up on the minornode ArrayList
					templist = new LinkedList();
					node.data = sInRec;
					
					// add the data for this segment to templist
					templist.PushNode(node,0);
					
					// push templist onto the ArrayList minornode
					MyNode.minornode.Add(templist);
					MyNode.status = "nothing";
					break;
					
				case "SGT":
					// Declare new instance of node -- it will store the input rec
					node = new LlNode();
					node.data = sInRec;
					
					// Declare a new working list
					templist = new LinkedList();
					
					// Insert node into working list
					templist.PushNode(node,0);
					
					// Check to see if there is already a "SGT" list in this loop node
					// The Exists() method will return the index of the segment if true
					bexists = MyList.Exists(MyNode,"SGT",ref pindex);
					
					
					if (bexists)
					{
						// If the segment already exists in the loop, then we
						// are going to pull in the existing list, add the node 
						// and rewrite to the minornode ArrayList.
						// Since there is no "Replace" method in the ArrayList
						// object, we have to remove the current list at pindex
						// and reinsert the new working list at pindex
						templist = new LinkedList();
						templist = (LinkedList)MyNode.minornode[pindex];
						templist.PushNode(node,0);
						MyNode.minornode.RemoveAt(pindex);
						MyNode.minornode.Insert(pindex,templist);			
					}
					else
					{   
						// Segment does not exist -- simple add to ArrayList
						MyNode.minornode.Add(templist);
					}
						
					break;
				
			} // *** switch ***
			
		} // *** ProcessHdr ***

	} // *** MainClass ***

} // *** End of Namespace: LinkedListMain ***


