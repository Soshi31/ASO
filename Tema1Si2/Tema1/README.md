# Tema 1 - Servicii sistem care ruleaza

Aplicatia identifica toate serviciile sistem care se afla in starea `Running` pe masina curenta.

## Rulare

Deschide PowerShell in acest folder si executa:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\Tema1.ps1
```

## Observatii

- Solutia foloseste PowerShell, conform cerintei.
- Afisarea este facuta sub forma de obiecte, astfel incat rezultatul poate fi redirectionat sau filtrat usor.
