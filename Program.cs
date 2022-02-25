using System;
using System.Collections.Generic;
using System.IO;

namespace DocumentMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            string newFile;
            List<string> filenames = new List<string>();
            int characterCount = 0;
            
            // If 3 or more arguments given, run in non-interactive mode. 
            if(args.Length >= 3 ) {
                newFile = args[0];
                for(int i = 1; i < args.Length; i++) {
                    filenames.Add(args[i]);
                }
                try {
                    readToFile(newFile,filenames);
                    characterCount = getCharacterCount(newFile);
                    displaySaveMessage(newFile,characterCount);
                }
                catch(Exception ex) {
                    displayException(ex);
                }
            }

            // Display “Document Merger” followed by a blank line.
            welcomeUser();

            while(true) {               
                int numFiles = 0;   

                // Prompt the user for the name of the first text file.
                // Verify that the first file exists. If not, give the user feedback and let them re-enter the first filename.
                // Prompt the user for the name of the second document.
                // Verify that the second file exists. If not, give the user feedback and let them re-enter the second filename.
                System.Console.WriteLine("Files to merge, blank link when finished. Minimum of 2 files");

                // Run continuously until given a blank return
                while(true) {
                    string inputName = getFileName(numFiles);
                    if(inputName == "") {
                        break;
                        }

                    filenames.Add(inputName);
                    numFiles++;                    
                }


                // Prompt the user for a filename for the new file containing the merged content you will create. In the prompt present them with the default of the merged names if they don’t enter a name (e.g. they choose the default by just hitting enter). Also, if they don’t supply an extension of .txt, append .txt on the end.
                // Example: Enter new filename (default: File1File2.txt):
                newFile = getSaveName(filenames.ToArray());

                // Read and merge the text of the two files.
                // Save the content to a file in the current directory.
                try {
                    readToFile(newFile,filenames);
                }
                catch(Exception ex) {
                    // If an exception occurs, output the exception message and exit.
                    displayException(ex);
                    return;
                }
                // If an exception does not occur, output “[filename] was successfully saved. The document contains [count] characters.” and exit. [filename] and [count] are placeholders for the filename of the document and the number of characters it contains.
                characterCount = getCharacterCount(newFile);
                displaySaveMessage(newFile,characterCount);

                // After the program does or does not fail to merge the files, ask the user if they would like to merge two more files. If they do, prompt them again for input. If not, exit the program.
                if(!doAgain()) {
                    break;
                }
            }

        }

        // Prompt to repeat the process
        private static bool doAgain()
        {
            string input;
            do {
                Console.WriteLine("Would you like to do another merge?");
                input = Console.ReadLine();
            } while(input == null);

            input = input.ToUpper();

            // Y or YES input, repeat. Otherwise, Dont repeat
            if(input == "Y" || input == "YES" ) {
                return true;
            }
            else {
                return false;
            }
        }

        // Display message about successfully save file and character count
        private static void displaySaveMessage(string file, int count)
        {
            System.Console.WriteLine($"{ file } was successfully saved. The document contains { count } characters.");
        }

        // Count number of characters (not including spaces) in file and return
        private static int getCharacterCount(string file)
        {
            int output = 0;
            string line;
            
            //open file
            try {
                StreamReader fileStream = new StreamReader(file);
                while((line = fileStream.ReadLine()) != null) {
                    char[] characters = line.ToCharArray();
                    foreach(char c in characters){
                        if(c.ToString() != " ") {
                            // add list length
                            output += 1;
                        }
                    }
                }           
            }
            catch(Exception ex) {
                displayException(ex);
            }
            return output;
        }

        // Read files in array and write them all to the output file
        private static void readToFile(string outputFile, params string[] files)
        {
            List<string> linesToWrite = new List<string>();
            try
            {
                foreach(string file in files) {
                    using(StreamReader fileStream = new StreamReader(file)) {
                        linesToWrite.Add(fileStream.ReadToEnd());
                    }
                }
                writeLines(outputFile, linesToWrite);
            }
            catch (Exception ex)
            {
                displayException(ex);
            }

        }

        // Write all lines from array into file
        private static void writeLines(string file, List<string> lines)
        {
            using(StreamWriter writeStream = new StreamWriter(file)) {
                foreach(string l in lines) {
                    writeStream.WriteLine(l);
                }
            }
        }

        // Overload for List instead of Array
        private static void readToFile(string outputFile, List<string> files)
        {
            string[] filesArray = files.ToArray();
            
            readToFile(outputFile, filesArray);
        }

        // Show exception with custom formatting
        private static void displayException(Exception ex) {
            Console.WriteLine("----------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.GetType());
            Console.WriteLine("----------------");
            Console.WriteLine(ex.StackTrace);
            System.Console.WriteLine("----------------");
        }

        // Prompt user for name to save new document under. 
        private static string getSaveName(string[] files = null)
        {
            string input, filename, defaultF = "";
            foreach(string f in files) {
                if(f.EndsWith(".txt")) {
                    defaultF += f.TrimEnd(".txt".ToCharArray());
                }
                else {
                    defaultF += f;
                }
            }

            System.Console.WriteLine($"Enter new filename for merged document. (default: { defaultF }.txt)");
            input = Console.ReadLine();

            if(input == null){
                System.Console.WriteLine("Filename cannot be null. <ENTER> for default name");
                input = getSaveName(files);
            }
            
            // remote whitespace from input
            else if(input.Contains(" ")) {
                // Trim spaces from input
                input = input.Replace(" ","");
            }

            // Default to filenames all together
            if(input == "" ) {
                input = defaultF;
            }

            // Check for .txt ending on filename
            if(!input.EndsWith(".txt")) {
                filename = input + ".txt";
            }
            else {
                filename = input;
            }

            // Check input to ensure it isnt blank/null and doesnt already exist
            if(verifyFileExists(filename)) {
                System.Console.WriteLine($"File already exists: { filename }");
                filename = getSaveName(files);
            }

            return filename;
        }

        // Welcome the user, or rather show a title and be boring
        private static void welcomeUser()
        {
            Console.WriteLine("Document Merger\n");
        }

        // Verify that files exists. 
        private static bool verifyFileExists(string filename)
        {
            bool output = false;

            output = File.Exists(filename);

            return output;
        }

        // Prompt user for filename. Give a file number to insert into prompt. 
        private static string getFileName(int fileNumber)
        {
            string input,filename = "";
            Console.WriteLine($"Please enter name for file { (fileNumber + 1).ToString() }:");
            input = Console.ReadLine();

            if(input == null) {
                System.Console.WriteLine("Filename cannot be null");
                input = getFileName(fileNumber);
            }
            // remove whitespace from input
            else if(input.Contains(" ")) {
                // Trim spaces from input
                input = input.Replace(" ","");
            }

            // If user gives a blank/null before we have at least 2 files, yell at them and tell them to do what they are told. 
            if(input == "") {
                if(fileNumber >= 2) {
                    return "";
                }
                else {
                    Console.WriteLine("Must have at least 2 files to merge. Filename cannot be empty");
                    input = getFileName(fileNumber);
                }
            }

            if(input.EndsWith(".txt")){
                filename = input;
            }
            else {
                filename = input + ".txt";
            }
            
            // Yes, the file must exist in order to merge it in. 
            if(!verifyFileExists(filename)) {
                System.Console.WriteLine($"File does not exist: { filename }");
                filename = getFileName(fileNumber);
            }

            return filename;
        }

    }
}

// Comments in the program are not great. Most are self explainatory. Sorry. :)
