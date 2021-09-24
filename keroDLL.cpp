// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
//#include "KeroCpp\Source.cpp"

#include "KeroCpp\Tokens.cpp"
#include "KeroCpp\Tokenizer1.cpp"
#include "KeroCpp\Parser.cpp"
#include "KeroCpp\Tree.cpp"
//#include "KeroCpp\tokens.cpp"
//#include "KeroCpp\Tokenizer1.h"
//#include "KeroCpp\tokens.h"

//initialize 1 only

extern "C" __declspec(dllexport) void initialize1(char* Text_line)
{
    Tokenizer_initialize1(Text_line);
}
//initialize 2 only

extern "C" __declspec(dllexport) void initialize2(char* input_file_path)
{

    Tokenizer_initialize2(input_file_path);
   
}

//token by token only

extern "C" __declspec(dllexport) int __cdecl tokenByToken(void)
{
    string Tokptr = "a"; string typePtr = "a";
    while (Token_by_Token(&Tokptr, &typePtr)) { ; }
    return 0;
}

//print all only

extern "C" __declspec(dllexport) void printAll(void)
{
    PrintAllTokens();
}




//initalize 2 and print all
extern "C" __declspec(dllexport) void __cdecl initializeAndPrintAll(char* input_file_path)
{

    Tokenizer_initialize2(input_file_path);
    PrintAllTokens();

}

// initialize 3
extern "C" __declspec(dllexport) void initialize3(char* value , char* type)
{

    Tokenizer_initialize3(value ,type);
}


//parse all

extern "C" __declspec(dllexport) int parseAll()
{

    return Parse_all();
}





































/*
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
*/
