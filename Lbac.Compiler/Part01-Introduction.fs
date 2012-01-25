﻿namespace Lbac

    open System
    open System.IO

    type Cradle(input : TextReader, errorWriter : TextWriter) as x =
        let mutable _look = '\u0000'
        let tab = "\t"

        // this is the "Init" function in Crenshaw's code. Here, it's just part of instance construction
        do 
            x.getChar()

        member x.look
            with get() = _look
            and set value = _look <- value

        member x.getChar() = 
            x.look <- char( input.Read() )

        member x.error(s : string) = 
            errorWriter.WriteLine()
            errorWriter.WriteLine("Error: {0}.", s)

        member x.abort(s : string) =
            x.error(s)
            System.Environment.Exit(-1)

        member x.expected(s: string) = 
            x.abort(s + " Expected")

        // Crenshaw calls this "Match", but match is reserved in F#
        member x.matchChar(c : char) = 
            match x.look with 
            | c when x.look = c -> x.getChar()
            | _                 -> x.expected(c.ToString())

        member x.getName() =
            if Char.IsLetter(x.look) then
                let c = Char.ToUpperInvariant(x.look)
                x.getChar()
                c
            else
                failwith "Expected Name"

        member x.getNum() =
            if Char.IsNumber(x.look) then
                let c = Char.ToUpperInvariant(x.look)
                x.getChar()
                System.Int32.Parse(string(c))
            else
                failwith "Expected Integer"
