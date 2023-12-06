import numpy as np

rawInput = open("day6input1.txt").read().split('\n')
times = rawInput[0].split()
times.pop(0)
times = [int(stringInt) for stringInt in times]
distances = rawInput[1].split()
distances.pop(0)
distances = [int(stringInt) for stringInt in distances]
def CalculateDistance(secondsHeld, totalTime):
    return (secondsHeld * (totalTime - secondsHeld))

def CheckLowerUpwards(index, currentCheck):
    if (CalculateDistance(currentCheck, times[index]) > distances[index]):
        return currentCheck
    else:
        return CheckLowerUpwards(index, currentCheck + 1)
    
def CheckLowerDownwards(index, currentCheck):
    if (CalculateDistance(currentCheck, times[index]) <= distances[index]):
        return currentCheck + 1
    else:
        return CheckLowerDownwards(index, currentCheck - 1)
    
def CheckUpperUpwards(index, currentCheck):
    if (CalculateDistance(currentCheck + 1, times[index]) <= distances[index]):
        return currentCheck
    else:
        return CheckUpperUpwards(index, currentCheck + 1)
    
def CheckUpperDownwards(index, currentCheck):
    if (CalculateDistance(currentCheck, times[index]) > distances[index]):
        return currentCheck
    else:
        return CheckUpperDownwards(index, currentCheck - 1)


def FindWaysToWin(list):
    print(list)
    WaysToWin = 1
    for index, time in enumerate(list):
        roots = np.roots([-1, time, -distances[index]])
        print(roots)
        currentCheck = round(roots[1])
        lowerBound = 0
        if (CalculateDistance(currentCheck, time) <= distances[index]):
            lowerBound = CheckLowerUpwards(index, currentCheck)
        else:
            lowerBound = CheckLowerDownwards(index, currentCheck)

        upperBound = 0
        currentCheck = round(roots[0])
        if (CalculateDistance(currentCheck, time) > distances[index]):
            upperBound = CheckUpperUpwards(index, currentCheck)
        else:
            upperBound = CheckUpperDownwards(index, currentCheck)
        WaysToWin *= upperBound - lowerBound + 1

    return WaysToWin

print(FindWaysToWin(times))

times = rawInput[0].split()
times.pop(0)
times = [int(''.join(times))]
distances = rawInput[1].split()
distances.pop(0)
distances = [int(''.join(distances))]

print(FindWaysToWin(times))