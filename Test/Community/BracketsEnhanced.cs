using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class BracketsEnhanced
{
    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        string[] expressions = new string[N];
        for (int i = 0; i < N; i++)
        {
            expressions[i] = Console.ReadLine();
            expressions[i] = "0T69T[PdnYQq6wowAG1(QVDwfPA0Ep7mHj9mfZ5zbvqSlW)ZoKdHWEwuQLOsaidqNXm(8dcqhyIsL)MnQZ9Z0koB6r)pFOV57s3OWi5MgZwqPQDD5R)Mt4Vmf7Fpg35e(yRuHicciRH3DAKf8zsryAIIa4iBk8UGz0eCPN}10}mVkbeZvdFA4GG20{lCRyxUowidCfZaGZOk<RgsApLppWrhmukx7R0vDzw<LUrilS3ez7YILUvtbKB>3LcBKakr5]DWe1sRohKPs2Am7IFTa7m5j1zAow4ik[qqJIk0TPSfM8jjMEEy5nBEPzy2mfUD9EfBLZub1sr0gEZCq6P29)63UxS7GAO(u8Px3A1RD2p)RDXSf0ZOI1II11VTHPGVbkPMcwRcWwOnFtV(Bb7JhS)sSB09HOSQHr7QYceyqX7n9ThRWR7Qf3{FzMii{1AGR6bC]DD99buwQHv]kyzvBjZUlD2QcaGc3hxKd2TKJ1d8K5vhNtYZoc2A3Rv76o1sUhLeVhlqk0MAIxZBKNH56w6bW466igJalhqj6sVsoYtdGJvIqwDVugDTjle]Hw4WV9XqwoevYtwtseZcxK2DrDqSv6KyvMV9rS2CLZtNpSNDAi4]6]jtWE>1TzqoTncmhWte4SZMaMjkI1vaSa]GJl6JowXk61mZW6o6IQ2ankF3J8iKx3vZRBY<eK5L<[0pZlzevm4O]855<b4BmcI)u3Q4TYcA)Pt4y)S9ao8yE9IsYQ(QljGMhL3JJ5ffWd<wWk0Gpes{fz0ODkQ4z5QcW5JW2gPR4nrYfIzMw1OLYWKhT6ppVW2zR7qQY0(Ocd5q7d4kbw3Hl7YZEslZdEUZFp58tPLTyjuhE5scFJiFMMA1Hb5bT{KO34JmI3oY8K3F331}A3W4xOnJ3HwkYk378UT)EJcCiOv}[v9PuacF]Xv]fGd1EE[HWZiC49spirgvO1OpAAFtROal1V]HXdqimWE]kz[WhOxUoPQ4KoVN]AKcWJJ5U9IGhMGn4ROzevL59AIzHebbXPCQ]e]qWqUsNzYxkIZofkDIIlR9d)xkLPbvZ0SzZJ5Dr(cip6WX1LLkM}CvJ1CYQOkwugKCQQLOD4lnpMueogweJEwOBBqBdF48DWAOlVrhlOYPjHBKJvGytXS0ZVeeSE2CiC2aC}UIJ}Odq<pwqS7<3UVqwD}vDB7z6tsysQPU82[1zRLFKvrUV7BhG6XLBUP]V2RnmTfEOx1(vZOqcT0Kv)x2Bfz(8xOw7Xso2qyg4Lyk3ogz7Iad)K[60H3wvbXjSqxabvKQlCCZ2oUruwvhHgkBm8oMNq6fz0FBOFB3p8u2Idl4[OjUtp(SBmt1v0oxSpi(Kg}9rNWq7KyVRui0iUd{VQNXE9h0juWNviNyLFb2nTfwufKHC]ZP>eNwYRjvjKaTVeA4H8Pbv8hZ0Il>Po(SslMiPh5bScZjXeqPGvTYo1kK(XxH6J6FLMMhInNeMoUwNRoO0ao2MXs3XBrNiXVRLR(R4f2AMy4tS0oVD6xO22GGS8A)Rtw}KU00SvMcw0vZfaWjgErVgglOm1WO9llmewGM2Y{iRkbqZxbNrAZVWctzctqbqlN4nu6t6fpYjAlOrd>cR88WQ6px]VKC[SCh8yv<9dKW7YJJQ01FBWai2Z<jy4RNX}KxU1MLFbS7e9Hg3Zxk2V18BaBOl{gkavrpmnqyP}{>hRFK250EfDZwPrh44H8G5gF20IzXvt<ZsbT25kXevzLIg92ZxwkQsdtBVh4sX>lCcLZPbI2Vl1i<yKRv8TGUVL4Lh4vRH<9VFVu37uwXUaaJUrVfCEe5)IdY(lg}NIrLM24kQ3fFwAP8abJEjejEbv}6iHX3o01(CWE8N7Q8V0]bxvX0usrkOCTa[Nsrc6u1(RloPLEIXoOWU013nBjrjiFc)yeGdVEocEmaaIW>PQMHVsgwvM0VtlxsO42]IujyiSl[Qp84qj<BrvthwX4ykuSSaPoI7GYX73tNFUY4bKLBU[fNlol]aeqtO4lRVcxXRVI0J8zNnL1HMbk2nXwbSo0wGPagS36IdW(L3jzvKMd3jik3uEi8n93DB6msXJtaAB5I)}fPUgwMY0ANmsRsLR6iZZyJ{UTRkRMGTO1wyPsfSWz(szEbGM3tR5gQeQJbLVSIwqYtAJ8SlRcM7A)6iUi>5u9oui7GWx8JlufzcVNLq2Dk2FU4HnjT4RWfq]]YYpf<Aw<JACCXkt1l852V<CJ8Zx}YsE]hhEmQPEifw[ibynlwYHje4NTQ0nJZ6PfNeXtOngrquxTM<Ao54RCSjRj38Krmagyrf6NLzWJIvPjwQ7V8UR<eE6Nz(rHkMABEUBagInpnyfk6sO3d(r8iKgi8bgLwfTXD(xLnexuD3(9YPRZCktnxIhQz9cA2venhdhcTU2ZicqGe0wFFct71LI9UX{EGstpo{WQURYAjoRl5ewux5bPx023s1oNb3caF76TrF0mriskPqbQO0Vov5hbEjrHJChEjJx3[URWbvzcDd5[hAPRVKI7{cEfRQMFniOjlVAOq0xePBLmFlBOR<f2ZBVX3fg9eyYcFWoG5<N9BxgViGxpy2FCIPl57HlXky10[qOWY4ZeEBi]P3cpFhJZT2hBDR21QkYvNqnIjbV4mBrkA(rO<wAkrgFSi8)J)JjdU1qzbBBVO)yMsqfbc780PvB[5C5D4k1HIFJLSRvnpjuGPwmR5Y33DIa1kmorz25cZIrQIwsSWG1sY7fqQcVGUq3cf4uZ4bgEn9]a8MyqSD3RrJPwnXsSj(KfcIVFm3K(PUMJM8k8tJVNkEyiSDTj)7UA6vPRrR2Lgi4r6UkKpWMgfhlr3CDSV5CzyJUWzQFbLoW84L2uARBVAV4f89mFkzvMeg))mDW)zQ3cu8ht0anjraH35AmcSJCaxFo7QQ8rbbavgaJYgWlg0PRo2Aajv4rN46jtYE7OLiCOptYZOnPhU22H)nXNRsL>sva0uAUsp3AwVQ2)qyEBndZuWdju947bofaIeZd5oTP67hSqyDlsJRhZhErRrhz0MGYqlKGvAH9VPJ29XZYBpSLYpbfkhfo>VkGyvCUyYtY7UCeiC2VsHMrh2JDPHg1byvcp6G5pBP6tRF3SRuX>b6Q8}8VhpoW{F>>P90kSBxMZIRF1)jP)><kUCVdl1pvLtEmWb1>SjbRotB>ZEuV60F)5yfsjUGvCIamXWE6]3NZjCIKAsX9og7fT]NBRfzn4KHqW}oFemZn1yOCFZD84c1wM4l5oQqQs3kxGPVvclvxwerKpY)LXzlkGYJLTGOfGYwdKGKWCkBYsW4nz5t(YcQv6SnFYaANwpGmyTKh5<T4oLPlPE{niQZmWqbu4iHAn0{<r>VjGKT(jYnS0jHsMrElBMREHWu1bWf6icFkFt97EHcUz5P8weVdBnF3eNuMe0y(GyMCbvnrPPBt3NJG<]pi7hz7nX7wCMcIlpsL9LhiQNvVIalOfeoHy6[S1B7qCf0LZ8wXzvIhinjkSIQdfUIZIw81vSk13ghSjhpL6xnXfILeEv<TJobd8mnngan0o1tzrfcjhKtJuHm9liX2SSpQjOgPmm8W(EJy8Dm4omSzTGxu0oiYCzXxRKSIyoWPoVZPLY)fAZdVC)sw)8MKrgENIYC9vbtkIPDLRHCsy4WX>do<5YQlZtA>3w4Auxr<p7PpmqB<e1BjkJNubMeMhyH6JLAq13HXhh7MZBrlu8bf6NS<FlRLydP04h76RUsPlw}VO0tV8we<fhkocingWhQ1Wk8eDNJKk4t>E386rTyv2h>SaGeIjdCD<PHS1tRnyejnFGgsoq5v>JSGeKPekIi2S<EOevQggHHMzRFc((sfSgYBwqw6Cq{lUcNY1zxO2rT6HbHtlRNv8JQuO3HXKWhNwiC3bdNnDJ}F{G4PAQHPv3lFxv6PMZAPEUtliCyKfpAh[t8yHQGYLrIhE1Dauqi[uhjkyaAFxUJ{eE{BWJcEstvkrW877yutvZWztQqoMAarmPVliQsykWBdrfe>ChY8bRrPBijMGpXyWtUa775y>7aIhmsDNj9XfjpyYkWRyEWxRwc2FCOQ6Djdue3YCOQArm3iF4WCLL0NYKbSJRCP0nx(nfxgXeQ2(cJtjUmQqFq6lWdpvzGDxfrjruP6p)KLNIo8EEbwLHL7We694K0do6uxi4CaypHTxR0BtuU9T6m5KhUEjSriK>THelhRLOWtYcgaw{7Ngbhkyn5f9{Tc}X{<wbHcNkBOSeEcZU3NDM}NxzRedjsTcs1r2m6nzeCeQ13RaHqsMvDFXU3RDbJxdaQ9ui8ydCvnIopqGv8IptM}6F83Gp1p}b5HdeRo2czy7HwUdUPMaU3MV)yIuQD)2Xp5vLwNeLyJ0EiouRopa32RvJ{JuC4Yt2}KZVxJ2lWvZRBbxKK0{9JA4VsJAP3LMh72bqQ0RVko{Ljiuaj0mtiNoGStAKBEOHx1LIK6tqL8GctnwZvEEvjimm71dGvmw5Y3LqcVCLMExIdMdxoW73zBX3SHYQi45cu5aKMyGMZauA}aG9cP2wHGXw{epFFQt4fGZ7nTzWgF<T5sdjzeQ9Yx1QkkCrJrrygIXFAsWaMfL>yoilMSORPTKCMYZXLRwnsN55ZieZ4WeWlpdOm3idmUojGaV<vX3<00<YmPX4igEUGSkkT9GniYzrB5JMSEAfEr2zZRJ5Ljxoa9elAHNrq5q15Fk1PJv6rCe7EpwrTE435Jnl5pHfyI0TsOOvOPvuLEFtGcbHo2Fzavf}EfJR3fnRPFQ8zAMNPMOr0jOBUt0aOSOe5JHJ2E{6VIHOmmRD6P86UZg57[ucG]zT35L1rOJGl}IH2i{bR8MmLXv0Hg>ZKwL61ccyqyBSgvtKTBEL2tmM2wfgeDp291HU9lLTNnPs8FY8n1ID199rbynQwcVqlPPiojWHTPRdvfrRlb<H0P49Da8BhY6DomwpIIPkv>ATj1DzNiFT)kW03Ujf3CFyEnWzi1JJdCmToi)Blk7QJThbaqgBFqH5AiabmUSNwGMrQpnlgEAYoyi9LoHkNuvE}DwY0[g8DdHna6[QpjZEM(USpBuQXWfHsniVWQOUvBLLo63zInLRAhR(2fptN)J7reQwaprvbaOYTvWGDjiLxnxKraovYr5qPr5FEJLcAWHdlb4t9u4J0RepTii8Y44j0fDzEo10sPUibXD1c7KPUKTm}}6zYs0oQliuAdTg1L35)nGWp9b1iqd9IttWhWpNYYqoFbmlEZ4nRGXU3aMa2V6e(ptmPkyp5RmZ1rYhr<faNOnzAZR7i9KYSGbWDyEnPRKRx4o0zF>pZ4NhXANA70es94Bl>TgF7aoevWxtEV}JFOKZW9Lzva32AIDLEY}safMNxKDmBF06nn7jzACLgwNilIyl>KGHiysW7)uMdsH9ErAe3TdT7A3bxSEpBz[oliLpVz7[M0mdb)tbYcxAEcd(I5VucB9kaenm)KMpGfvrDbjjjgAL1JXbuzafRD8OvSZQpF9pTL7X1uMGlRhWDZ9wV{UFvHDgl7ZIWAZ16UK}3uq5IRxPL54Z0eJAePy355Z2b<6sQUAx}cEw1r08tJgiikG7h<lrWTiHTXSanj74lFo>0WwMLtUZiBGG5X{wCQ7pKLgzku{O56Mh]J[Kg<>nlBTjjU{wvob4}Dht6qRdLdM4rJ98Ngdx9QctisD<jvkTkHCxA9XnFAXCRUYzUn>N4Ve<bvMdWWi130(g2R1UdG2)0ZTPf5Lf3d6tmGL8jE8FnuvlDJegr1SrjlzR}AaPyJkGPtU0Cyj}ecyMAL[OQbAqqQUPfmuEwAoCv]5MBi2{BiCXBJe}[Ksuf3ZhxjqgsvcKFlWrsyGa[cdi5hkG{Lusi2ZbRdg0aSTaAFlITtsiUbYPaqXJw2vgrb0N7HCQNg{N33wBkZS3SzD[4ECnu4Yf(s2qeN1mQ(C[aPZJ6DhJVcS(crGSgUOFvF5(LoTT1SwAS)G4PsSkoPuXgEtNGEdJMe8Qytilw>4ZydOPmKKI71zsldo[[iSPp8k>80qYko96uKflXdsfC79OU)SSF99UHwUwCSot]Ud4MQOIeAZAqDFUhGoMgxwIO3i8ULhIm6O4zZnohjwsHJdN4s4zXDRPQ2McwSezoL8hb5K]FrVL9YomDbNzRchlX9XdWW57Wfg]vxDd7tJ51Hp3PIQV15HUUXceVCs]YNUT{{MF2h7N8sam87jpVNn6yk>kjhBFdjNNWLUy<vKJrQiyFBYruSSG6tXWolZgYIOjCsoOwsjYUwAQPjR8(G7uvDIehbNh0nEVg(QmkzcdcY7FYtJtbn6q6YIqxnybQFkKoATSgx2lrwlBOYqL]LaxtK]CSogc5Kv{nIfUxDy8W8IjMII2QKIA2FhF}xbTLf4JzEEBlQ5gIHuN5wn4IUq48a5kCObHP3OPxcypvWFgQVwVq{oft1QTSOero9JMrZOo44eQfdsCfKF5MmnlIzc)9sEp2rHyHHJzr9tbLJaSWRiJXgt7(LevcaXZt0SDMRMC1zQrMWYSRmCi2{QtkwdchPqHDy0eC96X0BUqKFKK8Ihy[F[1g86W>BuREwncxn9lrk77Y0>CGa1m27op9fN2cIaliJOa6aVKjLCHbFmUz4SwagDhSyLdZRd5J4cX7mJGb<mer1tIUb{8P3O9WUulS91Oepmh5KR7us8g{oK18VnE1lnvsgSBSoIxWCs37{MW9}Vhh0KdJy5nufIc47lwWwTpyqJx<eEUJjsus3bKRnTMq0wytsPIwFhSM(w9VVK4ECq83xcfNx2RnNN6wg)MY06Xd8LHTdvRYwctjHttRlGiHN2iikznt1QvUGHex[bsJrukhcg]uSsy2fZiPcWywbffkB0cwQ8<FA3>NnnJzosM<Gxw]BJNpowtie5ZsgTbvTnG7XWW9Y12<luM9DnJzlPpjogBTeXEUCL9jeAZZiIPZ]FEH[z4Qer<Q1G02FTG5uI[x0TIj99xEDPT031A7m2A}I}hrYJ1VxWLlcm2REznbbXigC1K<uUa[udmhziB3yz8Y4B2el2I3hkktLFGAzJxmDVqXuPMea3qZPXHfiOxNG5PDgJ66cDI[0bo63T0MCR5XPxZ5xt2PuwztZwi72SfTb36HPtUO2MpN0qjRewkRTu<LvjcMvEq7uhc[LaLewhOe4wJ0sy]Rcbsa7TB<b4rJ6N9CN9HVWiOhjsJKB2KnD0aRKmuQnMWJ3Nbl4GUujJNRZM5GSPGRh>d2hs6fRAMemZ<9gZ3B1swkZVTNgsu0dvtnRWldx70pcKghMCEOjjivCoNu595BogxhUn)cO6)tpFiv6(AanwKox0j49yJxM0HgI(Ip[SfwKsBZ0W9NW7WML7muIItbsR]xX<rAf7yNLww3sW1B6UuonuhjnPury4bgrj4YWD9wHB8HfIr0ncZpyOsAvHzulbv8Cz>UIFCF5N3QoeOeFd5Adzdp]IASSVzbT4g]uYCr)BiN3mpyY(NZ9btjjQn69v1DIyQVxU>Gm(2hSzgnVukRA2I87lzuiHNQfjBCBfpdeWZVXh5a5ZCLdgRIBqUbs53G3bYVa5mamBRK}JZEl9IHYaYVpV2})6ksURG3HsheBzvBWNtzmhVrZpXIFxYEBS(WCEc7vpuRHB8(51rF8CbyALcSzpBSJcG8Bxnr6nhw>pl1lPSauuwr)ultXI1Ng0B92V6k5YXRnlroZcn6U0(DLfCk{gd6zgiHR4oJzv3<KvgnnVs>pFwrUrKo4a1VFJGHsyDMdu8W02}]lQS32Dbhj4CjI3brH6gUNt]drrgw32exbp5qDXQUydkw751f(Jws1vNuxDKfJ4VnRDCHzhRP4hZmGYBP5X7j81HU(zQDBKxa1p6ws5MHVK[gcdWbDqy8I9UAwnHfo5HNAkW[gZeeJoFmHC7P0N9yDiFhVon]0Rqe3K]D)PC9jeJMDHLg2Ui66VGWJMjoqUyaPMwOlD3bZeu26)<SpQ3P9ZR9OJDw<mlz>pKudSIMdZ59vpSB3b1PjP7rxFJ>Be3CtUyOWuB2T6ywyXJ49r3IRtSsRiIwo()aJXipRVQh0SsHLC5UAL]2Sr25h2jXhl[JgTFWKKZjOty4MvqCMtR1gixdw32d1zsfDT7eDG6SLQUPZbZa8BhjtFLz8xmX9LnC}aUIhbfhDjBZ1xKpD2SToP14lRQL}JtG]Y(Sjk7RqikFTNNJq4BeD64ZaTpAPJChcyvpC9iHdOJfddxkeIqW0ayM0neKHUhYtLVCdG3OlGTjrPfkgAL1DXRpGWQ7Pruj2GEm0KdIOtRL5iMVwBKW}YsnWTNm{fi4h)DD2l7TxR2YvAL0IdPvOvx0Hs2EjFP1QF6a8UNwVqmXcvUbtkNFBeX9fSeosfixoeSu0Jb>re4U5Vlnai8b74y7E3ILMRyXHAu4DV9e1zHQEWiN5c26iDNok1diMNJjZgNMfwvtZ>mTp6uk3B1h]fd4ZC5k]sKTi3GE35NoBk8wp1ghkRuGC2ks2GelLi63F>Bpw{5gYgoh4eGap8GHjbyZmwUN639uNc7bj133j27PTlWaDTlxCUjJwgiTULAbqid1m6tGjqKMTbv2b4LoF4WgTI2okEz7c5}9WDAU5K[iMMnapWr18DChA>gI1erTRa4<6ngiCk77mzQmmY8ljNFULzP{pnB0kqllyM}d{Rmrp88seB9hbF}Fj62c5TIar5Fnkg{avXLnQtHdUL8MucFWyK5Yt}TYQ6PGQ5yrpfS0VEf6VX3f7a5EPOYk1ii7Ek7yMeiJTrRYslMiIK9wIAUcTg46yf[4l))q5pfAdpV14vcpTHJFTa<0jr50k3Ekj6O2wrcyYqgV7J2A)nI4UCiSlp)m3DjB4oNFK5wVIFHjKL)sgvg0THDIrXOMm)qUugjLgdGHbTQT1iIkTG2yv44iSiJMzWB4PTNWFU<Cw0tBlzU4oQO]TC[MBvjRMFzX5UIq]a0p26VDwC48vRpS4SJnRu5zk2wxf4o7WCQbYZ6BfvydNCDQ[IHsKjWOmL(FmLsVCkiPwJ8EV1QUm(Ly76NIbSw(vR0bIAscs9omdt2vXQMnrHEAGqPjsqdBlissNuIDhp{aK2091l)JslURCULnpPBeAsPrLbwCXngfJ(s3ddfX3RAVB64gAiqq36iWItaQjWKjfqMmWRJGJaG39mh2q}4RTEGGNEYtY{O8bG6pJ8{NvRTanHK8[3SPQOhVqEhdK0casr4sRMQvB}l6dw2w1i{a8a{p1TYimvZbctPlO8f43pHMkHoRra9Ag0g11lQdsuJjT26ReV{7PHI26TMHV]}ko{Z1kdbhD7rLAwCLBwuScDkwK2dOU4Ka60EXRJehuAL)0vfxPKLyLryNJRki9aPu3>l9jsmWso5mRd3oHdXi1x>Fi5g1pBm0d8m1>Z4r>JPIdW0qsXkCk4uppLXK9[9Gg0]iHVOCD0IVKSmOwK4rP6iS4t8>q4CXTnWYMOuNpE62h}lnc1OPZUZsncCXldiftsVs3lABFGvAgbf}irAO8v4m6bo]jOI22gQnGwqOAftNYw3E[qU4feSx9Dm3Q3UdfHvF[bbZXAghRVO8hjlDqtkRNjtieaf<JO8oBxZOQnwFQaw1XocCg0U24rMr83lSZdSItPLxCuGb8cxL8LcqQg3oQ6kFY23O<H1moallZ5IbMyCUIXsRvV8uxAFCz6xKp5MazOSD2lEmKF082wlfv50IzVYlX0WWB4dc8tlQNOz3VCQxNkn][>cZhCMx31tVINEa<fFBoG3mcvppAUdNnHfYEc245a02h6hyUl20Qkhi75hFBCxrUl]UPR8Nk}b<<dGA77z98GlvBVetlV>6S>q5D2TWRxhq3e3vTe62yhh1mGB9dFvg2GqPQa6XmyCoxnv8eVdG6{TUWba1WMn)vi)kI1QhqOLMi[acyOMNSRf[f7ASyxy0AIB]NGHXm}dJJ0NFZnMOV0lu1FvPDknT3)ojjKIcfW(3h7gJfPZ<PESmmLXzpvRDscax";
 //           expressions[i] = "lUWWud[b()DM7Pdki(pF8tUb3lSqnBjl97slQJxhbl)ADCEYJdGYcr7EXa2N1rJLCNxi08)8iETg)(GqiUT}NmpDo4NpDftn91ZNCy1wuW7XhSZaxdp5Hj4KizFVlb}Oz<phDlTtV3DsgxWpniyBp7BJ2jrOngeRsE4mfqI45csblQ<fQ1Xm60<KJHzRRqilKxjm8MmBb5ooL15jFi>keTtzW4fBSoWgCkum9DsM51]BZQ[Oxd)B0a(zDG1hxXaCj67SE8AyIN2nYZbhIAo2CEbxqx2fMIGhgU7DXNwsQB)HuxwBUkSWZI4Xt8v()SGH{aTNBHoFyHP5rjBfHnZTwIWF8lUOjQHGVA8{5Ni1xq3E1S6rSpJeTuodfrwih]jry7HSQnp1Ufhkr9SZ4VhA7o0xKPj]3Zh6kd]03XLKZAEXxvRbhmzeHVqox1xW2Bo]6]C2DEajmDBime>lThGDnlFa55Vs0mM8LvxTYnXvRmTCYRu17ZNMvKfuGueVTFCk]Vqt<DtHOdhwf3HmmjdINueDTwb7pQABfwS1WNnJE1Oxi5Fl<witt7oY[6nhLf0ZqbhAo4JBYFGzpJRtL]E5bn4sXqkvTPC2ZQSfVL0oOB1mjP1LwrjT0t<6iepaspP4Y9TLM8EpJMIH2IrfaB2t)SJNHP)2)L3Ugyedv3g6sROPCkKgHZqiVmQwwjt(HDJpW7PeL7unpIabVtciktGlCal3p<VkC30nldY22kQO0IftfUEfTlAl3tI4u3w6Kt9KF1mt2Ywop{4m6p2CxBOO58G0rlxQtxckiORNy(2Dd2mw9C8h1LjJ{NWpKMzQcouG9phYyQ9lW1}TDIfWtfVsp5E)UZ9OcijZ1zwp32LzWY66x4u5rwgr}T3b41JCsavoH9w5[YWCKFnoyXpSOwyrYLiO5PJYUpllEmqWy2UNbiGHWXlo]d057Po][ZjzoxDQBrEyLy1v3sPZXn0cH2BC7wt73Zc4807g34KHSBMaq9vuS8l6QFAGqZcrhCgLx5LrEqLNUPmpum6lFnc4TqqVDTVOk7SiS9C]QCW]XARC0HaWOA7ng7RXmJn[jkT]cTrtO5IsqdbcQT1hJ8Dhg5owGkljdWdwl3oLyxfhuK]]sN4tqI4KwQMp8LG)G31VXXUpSxq0Gv3yv(80PYXmp9fM3fVD}86iBPJt}FcvEEsvIr}ax<mTc5IdSfM4QTdaWD<sBnN3vmQvo0wsmC}7TK[V5ErFhiUbzXeuMUMi5hhMj0yVMLlvxMLIvWXhgygr816foGHqe]Qblyvw(sF9g)5uSZU3uL(Id)VQLGPr[s9nNy6YFWeJckTdwldO[RjdtG1lIxDt5FVPxriqpaU(Fdb(}Ma4Dn9DBf{ui]ClVsDEYN6guV0ePHfhbkmdOEgRxjhOGOx6>gD1HHie1skTZgY>DckOs8HHllyDlj0RlnygQ(prxEOFQYMyYw(xC3EkMd1XPf3q2NpqIfvbYUmue1mQ(toRRHhKJFgIMEspA2uP24Tpoq)s1ENFP65Dhjpi4CRqqI9tOBfccGhvVpH2nZ8h5pQf3wCefrtostYaF4L0Frx12pl}M8zWGm{TDS83eHpAjb8zfnH1QhjAgEQzjlHlgPbFltGAaerl8XseYVARpUVw>NN4g1q]cNwzK3eEhlFIXKL8f8hekR2wT[D7dV4NuW<sANhKWquhvnDq<bzfqQw2ktt0OeLVdrg5yfETGIYKVZDl9bSdcdIf}5WkjGNiCylmLZdFyKHluzYsWCPU9{tW4d1}UuKe6ExJLSHW3lYMpgDzSZCaC8dTJ6ff6Kjmvqq4vQZQRi4wH{S9>JC2CRrqQHxAHMlDp<CBP7vLIYPp>n<audq26CmBQbBT9sE<AEv6FvwQ)0FeWVupBjM2Fe14bdFOr2x73qC(hpV}F10yMzqQbSTRyzW5ZLbdzk1pE}HTApl(8PbzX54IaDF]iS[XIBsBElxFdrCcWo11lbt(kirRHAXTfsP83bF3Ris3Wzw5wgE8Rii2HKo1G8)482vSQ55fLr3Ny9eT0qJagrpU74kxJduciFOAO0OBRubTDJltxED29wH4GXJRh1Z>DucamZEH0ncWxR4OEiCDyl]taLrjeXEiO7qv9ITTS[auejyI7GaNdeU<9zFwuO72r[bPrFYZ]J(Kz3YstelyM8Fad7DCqi0bibUAWSx9Dk0R)QpiOxNn7x7FIn3TMBvM2WflPBWP}neu2BP5KyJemhx2x1kJQrey9vxXT7R3KlujzzxmMt{TmhKX0R6c7m99(C2n1P9ZwZ58)lxOwKbzZt8KAiGH2E1UbfKzUpC3L>l]N7dyRDu2xuWtzyX9Nzg]3mof<ymXPYvEO<Fkk0VPiw5ACei<qhO3caVkBIk1rsOKEsg6wR3sJ}KDeAnFCOK6LKJ5SClUGpZnNeIuhFDfCXwmrUEuVu6b]tBFjjocSQ5[vPXX9a0Gk6qrBMGCbKCUxLsP9tK<v<(m1df(baYNi7(BJ(sbcrlLtOi80Mk6gpT3nh3ltUKDrQLuO{eLhRVo173akSKaF{A3[bwsakm3n[bGw13{NFdE1IjxZgWWzupjsch<<IE5GcNPKyhdMaBjLOembAFnqpmyEv2XAyu[Y](ir<E)wGEBVj2WIOhnZF12)03vgKzRHcu7RXSUqw9HrhMiXQ7mOQPGgHbwkLl2dAfK4)jGFLh[A2lab2TBHPWR1gIDUwkuzDbDDYtEZ09]RXQlevctkF3KZG9QdxMbg4MzmqhIdUPj48ckokqlgdmbysn4(jfunixdGZZ7lq6lgIoc9TDkXsrNQcknaZK6Joc331()TCMQ7IMp24ltehAz3Jak)mDuVD)NZn8OgrHMnNXJM6gC)QU4KMy)k5rA28QJ65>Uban68uJurOS7EEjZ0RpTjli2yH8WJ4ZmYLwrA4ZhCRkSYdNmWPgLRTQvPIxmT4oOYQMGs)n>j>S7UmIHW}I{XQBxj>>fSEkhvJiLSyiVW6iXF)leRKcbVrlVNHP3T)HfiPND2vKEk7LkqpcWGbfKJj8YaOuSETg9TxWLYkMUBN4uoGKJ0FXlKQHtL7S2y7qR7Oej>qfS<o5>RMJgX>8tTBaomQynCSHn)m7lbcENoRzvEIrtaiBN]PExxHxswcsZ1mDLhCyA]QUgbd2}E5iDO4yZVoWgh7vDp)Ru6LAQcmq8beRgm4hnRIceGyPhB26a(sp8MBnHbbPvepLo3<5k{9u1NVj{dHjcUVwb9y<kbXAVaRLjLsTQhk6xYh7vPgmwn2EHVoaqBW44H5s5qLLtFe7BdzZfDMTaPoM5QQgC7NBTgYdX>NZ5yy86WogxFuUQwabEg(my0G(A9eIsOiMuPRmFFAB8dugQUKC3lh3fJ3VRg7NMWXfQFALSyVNm4dDrA<aZ][9uOGT<Hvuojv9gMX1p9msjQF8LYSpQfMkyknXOyUF(fbZYkinNmh)zyWl)Mi0GC1KzJF3UN4Gw)tXjBgOeaVE1tQ6u5RVU><9adV>s8WFAIzv4jCUuaDPlAVoUWpJrGnLA0<STVlQnkbqf2DW6aOhsfcco<PO93QjhsXSJjWIsGs13p3KD2<68da0PGT}GjO9hCZdNB8dE8OYP1<N3zyrjdX3>s7n0THmDEwJ>05x9HZtT2U6pFM47uwRRkHSWJ9rmedbgQwtq<6bO42p6E46Hn8Ga31>UYCpIm<c0jKifbGAHoSdIAqjqUIVsq36E1lB21avr9beY4Nms6Vn78CKExs(JOVoP(BrTpreuXYE{PcPu7VRDsTVcqZF}Q{h8az[q[LBO1{amw3jRMx0FEeWlHlNHjFHONgDO{D>HjWzu16uBXrDiVnBv5PcJIlKycSI0bdjguKgp712MlNuOh1MXar7vmRjWRDnxReV4zPgXni>ZyaXJ(4iYJ0(Rj833ZGOilPrYELV)7AreUWTSUE3Ji3xQJSVVdmkcSceO4WcytzBFbuiPnmWIHkaPUj2jIHhjlzx67D>443wp{6ZXdcJS4UAOsyv4cQGuTGvfQO{WWYIk2t6znzPAgyrfa25bYaa7P7vIZ}A8{OdbChvxSeXKG3oX7L1fD<i2GG5Tu8JfOwlrP1NFeAnN2OR50nYpwh3zFOKV}WLAJF9nCENid6m}B}686VFjR)bhlOuWgRRF9ae){qrxPFwzs}xbI2ok15wGY3iuAIAqEJUgg{{TquiKEuvaUvKEC6nP}SQTLES{WRZ7kixClf5iEC9nutkTrbH<yekW6B><hu9<dS<U1E}ENY3mkycLrbKC3VqRaTS5{MnF3aTdcfKLPOCNi6rDyT4XSEfwRP3v0GsPH[6Ny3NRndXF5uIF5c]qd6DcE}BM99j2UB4caH2Fz7namdyebyKxfEDcFwdB4wqtPoQ0RuNfuTXV{4QIiSU>80vhgGb<4SIh7>pR8jhSM3A1vxUoIF9YQ)8PzjIYaXPBpKbPH)Cz0qwd6M4jhQ7SO0MRGnKLulhWWGSnu14Rl}hIZUZF1ulZmxsfeO[5tYXy4xbW5AOxwVgkENjKxBV1NWVUQ2hsD6fJgfkkwIvdZWrukQgIMf6XpxzIls0[4sK7NDsS1WTh0rXAxPJRhVAnYXqCv8BdTiqCdr(3Cms3vCNRibSL8LTjrYS7m5KHvNfSc(5pOQe7vQSFt9LXz3s4)1oez}pjDfC77Xco77W4pMHeMx3YDa4BtCrXnbLOGHBynLVJV5awZF6tWeReIkIekMHWDB4h}iJdVIJWvmmVarZM)Vjtdh4pfxWvHPQI(kqvMi<3bH33yoBDA1QSxlYbDmmaYJA2d8U33O5tmxHM1bWNR1kP3WnA6FswFbVz4S3m4DTeUJP6wuHWW0NhPS0CDIMYDqqmABh>nREoe7KdB89vp6P>FEn3o}RxSEwkP5ro0dhIft4WPoFGSQghkt}J>zZHVrIRB)CGvR[Ab1XvGp[XceGNwLcPbYxymfF6WwwVkxhyJJFZWk0mNWzQM0FhpGwRlhuiGYNCqcBwy)Ep2SUp681fH5LDrJblcnBGXiexKjX2k01vNyFiUXkG(Lb)cHQh7{XlVl8mKZ0vnTTCTA4a1J}ijyUWcvxF3mVdomGRD<GuLrUMw3AfWjzLURUrDU1QyLJvpLK3fqnsvGwktYQEtn5GdeYa}D3k4tcuD7<>ALBW60UtUpzmmrxVzg10VGc22s56VV9w8SIHaQGY84iFa{LXKF6ELSmRcXZvavjJSHOOO{L3CH4TpwANRrlbI]XAUiln1iFyu[I6otSmZ2uXZ16ryb<uS2>rYP{ptStuE1}PdCdN<AjKlKEbIoPmaYhZLHL>f2VoK<MUyozMI(xuPppnX6R8u72eMgSMhRm9jrJ1JfQ8)Us0gb7O61LePIALz9hWgldCe4cXH}JkW3UAyA}0w[nV5d9fXIA]IoI3aaBDqUBT{gfMuzCtBMv1Ks4p5jgNM71cA2je5pYQBS9DdH9IbKMKOBDwEMjlFRNvv8LObNkLr}URBKuPfMkqakgYUe7qx8[bqFBu1R[XgH0CkIrzvNEZ67mmNbez6EbxQs971cmWewm7{4SL{HvyCw2PJd[r1uN(eu4cu5kIU14vqZ3lLjmGMsZuEbwdPQvKbrOTajBKLoSJkc15hWxNjWFzKqCOPSW4pQXlfwGT2jeOONy9UnyuOEF(K6A[RKl(orfnLn00PaJbs4Vc33k3dQCcyHcazHxVzom8TYC7tKpj(EkkhSBJ8zwpgm6RUwro9YqAoRE)>pP[6[5j9g36cq5BybdhAxFGC8eZTfBnsICPtl>UyH3J1ejRRG1)]2DjLC8x]2dOi81eQa]xbCdCFD3mikPBtxIryEjiCFVr8JBYhwTTm01L4feXUZ7qBXDqyb6rRwP]P3573SM8NYZ9rU4Ko4bGPRbjqfgXmuPf{oGKKgplhmdIH{FeuGcCzxT><LFG1Vj(Rws1a(O5z7cbsI6BGAFPYpbUaIEI9U4qA9GCEvWspgf5]OXYYI0oZckLultDmD8R]rGGaYdcBIj380kCKRdKsl77UZK7VSGeebFoFNawE8mQPV8FWY3HU9jkM1boGKa{9libt4Y}hOKIzDv7eJ{9yVXocmdbBW3208TBbEKwBCsb)5dso(LwIoW{30QRaYteiamgLGUWvYpoRX4lkH09n1kYpwuOomjo03VI[hlevrWZfGsmSotiEpqolh[jZo0wO3QAalo7Eehd0T>EgsoyUXjZ2cckGSgJHAK86Y8QzsvY3ahF>4zu5sGlmt0n8xHktLhfiYOtDqx6K1sFJJl<IiUR3k6wNyfei5ASdO5{KPfKhYWZ0LLmtg4IWB9E0EqRpigCZLV0QN8WUHnxuW2EhMKR8odPpa75yn6{qAYZtfwFWQGFeIG3u6{OcUZRdtSmrUeStrHqTZHdyhQlopd2hZt}94HrHLoK6N4Sxm4lm<RqgEmgh6F6SyR55RN6rdWJF8V88pbFYrwk4Zdb(bo)MI9HUPrUdqBfc7qoTPF9r[a0Shpu3Czz7vRuro1Gx1m]tUTYfJrc4am<CX9RpbAt4ZXTsV6OOCUgaMhczWZqccSc3Pshd9iV2YNV>ShW492kCgMoXYoQf<uID7gQJALrfdUyM]yHk170Gln0laFUWjfJwV1jc2779d8Hg8S42m0<iP8fYIrGbCst0pyEoMZQzTBFrBRzofv9niMitG5gY]Rg6NBs4uNa[7L4RKyAlSk1dwqwM5pzwrgHdV80vPOdFsCm6R5qXCl1jXJQ4U7td<AG7SYKr0FR2Mm8gRu[6gOYEB8Xxu}I}V4Q4ON4yoy9jGqDovQpZwwSvnn2h<yonl7bwg12T666E[[tTQVzAqTXhP9NTF7p6uEtj5V5HBJ<KVBykdwmAcZsWIb8l95RmqO[CDoxBOHS]1Dd<pw3gSsfQQ>nVkR<nd9GxtpRqL)J1Q115U2RqdlzooevZgLY72GrhO38T75sWXmjwqKEZ5mKr07GG)TXxk93M(LgyqmxyIo(kmV9IgBnPFPye1W[HFXWgtRVJ2HInYZE1sY27MRXDrCHzMTH9x]gAfk3RP51kfbzFRSR0h2XgoKLK0N90gNz0NVBtag6iUBOcisDhCzwX42GXtf<pRywdd2sB>AcAqWhiCxXLijFqkM1etd2FgRdyspNKp]zbSiAUqU2FNf4BTZyjILFB5HKSo6ccta]xkFcM800Uk6rX)l(b68NfFivBftQbLRBO459mF>SnffmnRhty(}en}gUzZi9Bg5bhkbo2UV8Szl39IgXfsXDBev66qPyJC)(upFrLKv3f6kj2JK(Ky9kBwyd5KscE9ICaFYP1ylYfzRX0TDuoMVvp5bbvVK>11CESPe3BUvOFfDx3KLx7muOx1nMuKpR2WFzyR4dKBpBgmlGGLsKBehCHbbXTe1OJi2TqPZCViygEqOrFYIuOEb5LT)jBoXcsnDTF7YmK9mhVOB(TxkHxU92qqj{gI1<R9eAyVItTwio>I5FPsEIzTYX}ejLRUUx0a5fPx2stFsueGzZtYF8Oqy]YERFQNXc62vj0QUElqCCyn1EaCrv6GIeGpgN4JwWDsw]vi0GupE5LnPv(UOyEgCWSwvAKGGPn7m([Du2uF9Ub6swFc7KhG604L92[kUbjXigjK3idQVTpFpDDf0iR2c9oo3A8iD2AqjI4d]70OAxNTal12gRnrrbXmlBViCS2S6xJYf]hTm3vq0C9vQzDefWiVQ5pv5fGMV81eCaAkmV9qMX6a)7Hriw2aXgJW7vSynnyKHNyiK)3yMXX7eIu7EYaWvJ3yldNaAhOwbTy<Y1fLZNmQA1zkkz2cUkBUpkPaIrBNE2af4QAlgjSvhHoWAH<1b42cQOSeYJkbqfYKORgLUT5P5loscwF37ABJheRWV5Uro1YANGVsChwx2CpY6kqdd2btghXN3UZLlcquqou>FgVnmflLvc01vUYE36>(mRT1RjuD3wbwgwGnsZXKRAyALgc5s5EJDavpMD7eE33)LCRYdDLWPCZf]xrCPwwFF4WG1Z5fV[CNh9qSzyRJQa28UGn6LVxiRii23XxYAglOPQWsgwD4VEDXtwfiVb4fUoqL5KhcBQhsMY3AxqOTwcc1D8qJuPzl3Jrj1dus2GPJlM08kcfV7lAOacSmix1vgiN8kmgo4iCiTxCbs95uUXAKViUJ0SQfa2a6pejL7cCk1votpJLkso4FBywZoCp9xXJoaq46DsNABI2}UsT5sMK3}MIUJ4wDmXpoQ17uID08m1DxiOu4kWtJq7LEPkdSb7G8Dv3AH1qkXM]867BqgNnTjOYX62yt4VXi6KD5ymSon5SUuTcubj9aj(EErsN5MURllQ6Io6h1y5kwVq}87IBHP{2hfMPvB4z)eKDeAEREQ2NdCQDuN8XbrmTnks9Agv5e0gMDBg0GJGCUbcMoHPSb0KmfXVtRfB0>ljYsvcD7LzhjUJs>ST]FYFuElUSNaD9SPmSJnV59In7jgdbH8W8vDlFr1pXYgnrTGehUfC845fnF2Q2SsoZEiE]OUCYifffEKhqwnBddhwBqnK6w78ZjGc00etMjMzMqLaE7JnbN9DfzUp3P>eWF5Wf8MdpqEqQnnb{0x2onXE56W78m6XQmv37No1Gf25BiQ1k9XAdeMCc6sxpF3QJHH5L9}lvbjU0BE0UDrmJr9mvSk5ckILfTKzWA3qfbM18X[uS>70i7IB8lVxAVzWAnyNUJi8ird8IbQ2t1HhKe2wEhV<1kh{eJnHdkQyRPxWGjQS}V{UR}T6Lg4Kwn6{3rE49vJerEawTqdhfSSJNJIwiWk1Hkb5ZCM4RER11WNz5A04aIo5u}qFdxQtvbbU5pkCrNSzeHMVwseO[jCQIfAX)PjDdydhvPdh)276ihaHU4ljd<m0lxqd5kEmqA3pBT0adSX6qFk3mTtlbyhIcm4sxTsdwQKzhmm8OzVXEhQ1VlNcZjKpqHEVmlUe6vIYYc04r0W)nBTTWnTxOvkMGdfVQSmwZFNRD6s8h)dX)kqW6T67Pnh)KB<0M6v0O5eK9ELBLaJ3Jmpn9UaAeLZ0NFb]L5b2nG4hKhgbkLuGTfFLz2E8xQIvFPHZ1ELe[ADO]c[PaTgHX4rRI1dKAK(pdFSLaqAJiUWGlNrfno2(6J52bwJlDDl({kZc3fkllYD)SJ(uHvFPFNxQQWPTzyeFBArgNBxUN72IgxlY0iqQCaoolwnBB}wjW745U02olvXUzx2k1ZjUf5{Tij{Agt[i43QmuUZwI83KqEffjty}Rm{5qgZ1RqCDZfuykz4zZTmWDxGiNt{Jrslc6CpHovTLvjpC{Rl3b]5e4eHgkLhu}vOc7MvkXy0Ue24Pk{D0Kmj5Gs)kFoLJeLgsuLaE56lxh6uivX6U1nP5vs7>yVGWgmAhaEcuZSpXcORv6AAopa8xO6vH1a>VLM6F05qs>lg5CisDpm8>fZZXF7FwXvIgh9FeZm0Ojo49[0djDHa0Vy7zaqS3orV0ijx34WM3zmdLzZNjQoYrpKGmJRo5QDcBcbfMiSwVQUnbpnwO]4GPd35907wpd0N>qdoSuQxBluq9vISDmIJ3VAkfQytg31nKPwg}YNk1zEGK}GZFhD6AbxlRiY0]NjY9[cuFzaH74GP012Qz[yjhzh4EihqvYB50xGeWkSKYm<0jF8f1y8hBQbwSIrzYAg5lIxeWhKmDz4c4V<aBmprSIs0YmNKmFa5FrI5TTlIXlzOkCWWAic5U]XO[Y>SNb65N39xDwYzFI6mJc4ebqe0T1kCQNsei7iC<W9t9oycQtxZ]i8jbPSWEZ3IR1gpv}Upz650Vak<lnZh<D>YKt9gL22f47xnL846wmodPZeRnKtdyfA7yV5WXLSRzT2pm5m08a2B9nUk4kGEAsH5K7837z>9GGsNl6BdU49{7)Bikb4cVz2nQd)ugbcSwUrVBWLEsOAnxrmt[Dqbg[yRPzEj]8kveWxok>reay3fudfSCA3fSO2wIBvUjG507OsF)NE3jP9CpOzmJGMP0(o48FRECT8SUC<4A4";
        }
        for (var i = 0; i < N; i++)
        {
            StringBuilder simplified = new StringBuilder();
            int nbParenth = 0;
            int nbBrack = 0;
            int nbCurly = 0;
            int nbAngle = 0;
            var next = expressions[i];
            var counts = new Dictionary<char, int>();
            counts['<'] = 0;
            counts['('] = 0;
            counts['{'] = 0;
            counts['['] = 0;
            for (var j = 0; j < next.Length; j++)
            {
                switch (next[j])
                {
                    case '<':
                    case '>':
                        counts['<'] = counts['<']+1;
                        nbAngle++;
                        simplified.Append(next[j]);
                        break;
                    case '(':
                    case ')':
                        counts['('] = counts['('] + 1;
                        nbParenth++;
                        simplified.Append(next[j]);
                        break;
                    case '{':
                    case '}':
                        counts['{'] = counts['{'] + 1;
                        nbCurly++;
                        simplified.Append(next[j]);
                        break;
                    case '[':
                    case ']':
                        counts['['] = counts['['] + 1;
                        nbBrack++;
                        simplified.Append(next[j]);
                        break;
                }
            }

            // fast check. no need to parse if trivially not possible
            if (nbAngle % 2 == 1 || nbParenth % 2 == 1 || nbCurly % 2 == 1 || nbBrack % 2 == 1)
                Console.WriteLine("false");
            else
            {
                // text
                Stack<char> brackets = new Stack<char>();
                Dictionary<char, int> current = new Dictionary<char, int>(4);
                current['<'] = 0;
                current['('] = 0;
                current['{'] = 0;
                current['['] = 0;
                int j = 0;

                Console.WriteLine(BracketsEnhanced.ScanBrackets(j, simplified.ToString(), brackets, current, counts) ? "true" : "false");
            }
        }
    }

    private static bool ScanBrackets(int j, string next, Stack<char> brackets, Dictionary<char, int> current, Dictionary<char, int> counts)
    {
        while (j < next.Length)
        {
            var nextChar = next[j];
            char altChar = char.MinValue;
            if (next.Length - j < brackets.Count)
                return false;
            j++;
            switch (nextChar)
            {
               case '<':
               case '>':
                    nextChar = '<';
                    altChar = '>';
                    break;
                case '(':
                case ')':
                    nextChar = '(';
                    altChar = ')';
                    break;
                case '{':
                case '}':
                    nextChar = '{';
                    altChar = '}';
                    break;
                case '[':
                case ']':
                    nextChar = '[';
                    altChar = ']';
                    break;
                default:
                    continue;
            }
            // is there a bracket
            if (altChar != char.MinValue)
            {
                counts[nextChar] = counts[nextChar] - 1;
                if (brackets.Count > 0 && brackets.Peek() == nextChar)
                {
                    var copy = new Stack<char>(brackets.ToArray().Reverse());
                    var currentCopy = new Dictionary<char, int>(current);
                    var currentCounts = new Dictionary<char, int>(counts);
                    currentCopy[nextChar]=currentCopy[nextChar]-1;
                    copy.Pop();
                    if (BracketsEnhanced.ScanBrackets(j, next, copy, currentCopy, currentCounts))
                    {
                        return true;
                    }
                }
                current[nextChar] = current[nextChar] + 1;
                if (current[nextChar] > counts[nextChar])
                    return false;
                brackets.Push(nextChar);
            }
        }
        return brackets.Count == 0;
    }
}