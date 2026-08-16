// Sprite Glow Shader — Unity 6 / Built-in Render Pipeline
// Replaces the old ShaderGraph (UnlitMasterNode) which was removed in Unity 6.
// Enhanced with HDR support for Bloom and Pulsing for collectibles like Stars.
//
// Graph logic reproduced:
//   1. Sample _MainTex with UV0
//   2. Read vertex colour (fed by SpriteRenderer tint)
//   3. Lerp between (VertexColor * GlowColor) and (Texture * VertexColor * 2)
//      using _GlowIntensity as the T value
//   4. Alpha = texture.a * vertexColor.a

Shader "Shader Graphs/Sprite Glow"
{
    Properties
    {
        _MainTex       ("Main Texture",   2D)         = "white" {}
        [HDR] _GlowColor("Glow Color (HDR)", Color)   = (0, 0.816, 0.934, 1)
        _GlowIntensity ("Glow Intensity (0=Full Glow, 1=Normal)", Range(0,1)) = 0
        
        [Header(Pulse Options)]
        [Toggle] _UsePulse("Enable Pulse", Float) = 0
        _PulseSpeed("Pulse Speed", Float) = 3.0
        _PulseMin("Pulse Minimum Intensity", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"      = "Transparent"
            "Queue"           = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType"     = "Plane"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off        // Two-sided (matches m_TwoSided = true in old graph)
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   2.0

            #include "UnityCG.cginc"

            // ── Uniforms ─────────────────────────────────────────────────────
            sampler2D _MainTex;
            float4    _MainTex_ST;
            fixed4    _GlowColor;
            half      _GlowIntensity;
            
            float     _UsePulse;
            float     _PulseSpeed;
            float     _PulseMin;

            // ── Vertex Input / Output ─────────────────────────────────────────
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;      // Vertex colour from SpriteRenderer
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                fixed4 color : COLOR;
            };

            // ── Vertex Shader ─────────────────────────────────────────────────
            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            // ── Fragment Shader ───────────────────────────────────────────────
            fixed4 frag(v2f i) : SV_Target
            {
                // 1. Sample sprite texture
                fixed4 tex = tex2D(_MainTex, i.uv);

                // 2. Vertex colour (SpriteRenderer tint / flash animations)
                fixed4 vertCol = i.color;
                
                // Pulsing multiplier
                float pulseMult = 1.0;
                if (_UsePulse > 0.5)
                {
                    // Basic sine wave (0 to 1)
                    float rawSin = (sin(_Time.y * _PulseSpeed) + 1.0) * 0.5;
                    
                    // Apply smoothstep to ease the extremes (makes it linger softly at the top and bottom)
                    float smoothPulse = smoothstep(0.0, 1.0, rawSin);
                    
                    pulseMult = lerp(_PulseMin, 1.0, smoothPulse);
                }

                // 3. "A" branch: VertexColor * GlowColor  (glow tint)
                // We apply the pulse multiplier here to throb the glow intensity
                fixed4 glowBranch   = vertCol * _GlowColor * pulseMult;

                // 4. "B" branch: (Texture * VertexColor) * 2  (standard sprite, 2x bright)
                fixed4 spriteBranch = vertCol * tex * 2.0 * pulseMult;

                // 5. Lerp: 0 = full glow, 1 = normal sprite
                fixed4 result = lerp(glowBranch, spriteBranch, _GlowIntensity);

                // 6. Alpha: texture alpha * vertex alpha (preserves sprite transparency)
                result.a = tex.a * vertCol.a;

                return result;
            }
            ENDCG
        }
    }

    FallBack "Sprites/Default"
}
