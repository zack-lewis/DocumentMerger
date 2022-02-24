using System;

namespace DocumentMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            string newFile;
            List<string> filenames = new List<string>();
            int wordCount = 0;
            
            // Display “Document Merger” followed by a blank line.
            welcomeUser();

            while(true) {               
                // Prompt the user for the name of the first text file.
                // Verify that the first file exists. If not, give the user feedback and let them re-enter the first filename.
                filename1 = getFileName("1");

                // Prompt the user for the name of the second document.
                // Verify that the second file exists. If not, give the user feedback and let them re-enter the second filename.
                for(int i = 0; i < numFiles; i++){
                    string prompt = (i + 1).ToString();
                    filenames.Add(getFileName(prompt));
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
                wordCount = getWordCount(newFile);
                displayStats(newFile,wordCount);

                // After the program does or does not fail to merge the files, ask the user if they would like to merge two more files. If they do, prompt them again for input. If not, exit the program.
                if(!doAgain()) {
                    break;
                }
            }

            Console.WriteLine("Press <ENTER> to exit");
            Console.ReadLine();
        }

        private static bool doAgain()
        {
            string input;
            bool output;
            Console.WriteLine("Would you like to do another merge?");
            input = Console.ReadLine().ToUpper();
            if(input == "Y" || input == "YES" ) {
                output = true;
            }
            else {
                output = false;
            }
            return output;
        }

        private static void displayStats(string file, int count)
        {
            System.Console.WriteLine($"{ file } was successfully saved. The document contains { count } characters.");
        }

        private static int getWordCount(string newFile)
        {
            throw new NotImplementedException();
        }

        private static void readToFile(string outputFile, params string[] files)
        {
            try
            {
                foreach(string file in files) {
                    //STUB
                    Console.WriteLine(file);
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        private static void readToFile(string outputFile, List<string> files)
        {
            string[] filesArray = files.ToArray();
            
            readToFile(outputFile, filesArray);
        }

        private static void displayException(Exception ex) {
            Console.WriteLine("----------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine("----------------");
            Console.WriteLine(ex.StackTrace);
            System.Console.WriteLine("----------------");
        }

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

            System.Console.WriteLine($"Enter new filename for merged document. (default: { defaultF })");
            input = Console.ReadLine();

            if(input == "" || input == null) {
                input = defaultF;
            }

            if(!input.EndsWith(".txt")) {
                filename = input + ".txt";
            }
            else {
                filename = input;
            }

            // Check input to ensure it isnt blank/null and doesnt already exist
            if(!verifyFileExists(filename)) {
                System.Console.WriteLine("File already exists!");
                filename = getSaveName(files);
            }

            return filename;
        }

        private static void welcomeUser()
        {
            Console.WriteLine("Document Merger\n");
        }

        private static bool verifyFile(string filename)
        {
            throw new NotImplementedException();
        }

        private static string getFileName(string prompt)
        {
            string filename = "";

            verifyFile(filename);
            throw new NotImplementedException();
        }

    }
}
