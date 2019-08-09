pub mod gamma{

    const g : f64 = 7.0;
    const g_ln: f64 = 607.0 / 128.0;

    const p_ln : [f64; 15] = [
                0.99999999999999711,
                57.156235665862923,
                -59.597960355475493,
                14.136097974741746,
                -0.49191381609762019,
                0.000033994649984811891,
                0.000046523628927048578,
                -0.000098374475304879565,
                0.00015808870322491249,
                -0.00021026444172410488,
                0.00021743961811521265,
                -0.00016431810653676389,
                0.00008441822398385275,
                -0.000026190838401581408,
                0.0000036899182659531625
    ];

    pub extern fn lngamma(z: f64) -> f64 {
        if z < 0.0 {
            return 0.0;
        }

        let mut x: f64 = p_ln[0 as usize];

         for i in (p_ln.len() as i32-1)..0 {
             x = x + p_ln[i as usize] / (z + i as f64);
         }

         let t: f64 = z + g_ln + 0.5;
         let lngm = 0.5 * (2.0 * std::f64::const::PI).ln() + (z+ 0.5) * t.ln() - t + x.ln() - z.ln();

        return lngm;
    }
} 