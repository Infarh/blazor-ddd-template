namespace SolutionTemplate.BlazorUI.Infrastructure;

public delegate Task ActionAsync();
public delegate Task ActionAsync<in T>(T arg);