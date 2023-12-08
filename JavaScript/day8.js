const fs = require('fs');
const { arrayBuffer } = require('stream/consumers');

var input = fs.readFileSync('./day8input1.txt').toString('utf-8');

input = input.split('\r\n');

let instructions = input[0];
let instructionPointer = 0;
let stepCount = 0;
let directions = {};
let currentNode = "AAA";
let currentNodes = [];

for (let i = 2; i < input.length; i++)
{
    let split = input[i].split(' = ');
    directions[split[0]] = {};

    if (split[0][2] == "A") currentNodes.push(split[0])
    
    let lr = split[1].split(', ');
    
    directions[split[0]].L = lr[0].substring(1);
    directions[split[0]].R = lr[1].substring(0, lr[1].length - 1);
}

//Part 1
while (currentNode != 'ZZZ')
{
    currentNode = directions[currentNode][instructions[instructionPointer]]

    instructionPointer++;
    stepCount++;

    if (instructionPointer >= instructions.length)
        instructionPointer = 0;
}


//Part 2
let startingNodeDetails = currentNodes.map(node => {
    let obj = {};
    obj.ID = node;
    return obj;
})

//Find number of steps required to loop back to Z
for (let i = 0; i < currentNodes.length; i++)
{
    let count = 0;
    let pointer = 0;
    while (true)
    {
        count++;

        currentNodes[i] = directions[currentNodes[i]][instructions[pointer]]
        if (currentNodes[i][2] == "Z") 
        {
            startingNodeDetails[i].loopValue = count;
            break;
        }
    
        pointer++;
        if (pointer >= instructions.length)
        pointer = 0;
    }
}

let part2Steps = 1;

//Get the prime factors for each node
startingNodeDetails.forEach(node => {
    node.PrimeFactors = GetPrimeFactors(node.loopValue)
})
//Find and remove common factors
let commonFactors = startingNodeDetails[0].PrimeFactors
for (let i = 1; i < startingNodeDetails.length; i++)
    commonFactors = commonFactors.filter(x => startingNodeDetails[i].PrimeFactors.includes(x))


startingNodeDetails.forEach(node => {
    node.PrimeFactors = node.PrimeFactors.filter(x => !commonFactors.includes(x))
    node.PrimeFactors.forEach(factor => part2Steps *= factor)
})
commonFactors.forEach(factor => part2Steps *= factor)

console.log(`Part 1: ${stepCount} | Part 2: ${part2Steps}`);



function GetPrimeFactors(n)
{
    const factors = [];
    let divisor = 2;

    while (n >= 2) {
        if (n % divisor == 0) {
        factors.push(divisor);
        n = n / divisor;
        } else {
        divisor++;
        }
    }
    return factors;
}

